import os
import json
import asyncio
import aiohttp
from aiohttp import ClientSession
from bs4 import BeautifulSoup
from concurrent.futures import ThreadPoolExecutor
from urllib.parse import urlparse, urlunparse
import time
import shutil
import logging
from urllib.parse import urlparse

DATA_FILE = "data.json"
FAILED_LINKS_FILE = "failed_links.txt"
TARGET_URLS = ["https://www.idnes.cz", "https://www.ctk.cz", "https://www.novinky.cz"]
EXCLUDE_PATHS = ["/wiki", "/premium", "/archiv", "/sport", "/volby", "/ucet", "/soukromi", "/diskuze"]
ALLOWED_DOMAINS = ["www.idnes.cz", "www.ctk.cz", "www.novinky.cz"]
EXCLUDE_DOMAINS = ["www.hovino.cz"]  
MAX_FILE_SIZE = 2 * 1024 * 1024 * 1024 
MAX_DEPTH = 130  
CONCURRENT_REQUESTS = 20  
TIMEOUT = 10  
BACKUP_INTERVAL = 1000  
BACKUP_TIME_INTERVAL = 60 * 60 

visited_links = set()
all_links_to_visit = set((url, 0) for url in TARGET_URLS)

executor = ThreadPoolExecutor(max_workers=50)
failed_links = []
collected_articles = 0  

logging.basicConfig(filename="crawler.log", level=logging.INFO)

def log_message(message):
    """Logs important messages."""
    logging.info(message)

def normalize_url(url):
    """Normalizes a URL by removing fragments, queries, and standardizing format."""
    parsed = urlparse(url)
    return urlunparse(parsed._replace(fragment="", query="")).rstrip("/").lower()

async def fetch_page(url, session, retries=2):
    """Fetches HTML content asynchronously with retries."""
    for attempt in range(retries + 1):
        try:
            async with session.get(url, timeout=TIMEOUT) as response:
                response.raise_for_status()
                return await response.text(), str(response.url)
        except aiohttp.ClientError as e:
            if attempt < retries:
                await asyncio.sleep(2) 
            else:
                log_message(f"Failed to fetch {url}: {e}")
                failed_links.append(url) 
    return None, None

def is_allowed_domain(url):
    """Checks if the URL belongs to an allowed domain."""
    parsed_url = urlparse(url)
    return parsed_url.netloc in ALLOWED_DOMAINS

def parse_page(html, base_url):
    """Parses HTML and extracts articles."""
    soup = BeautifulSoup(html, "lxml")
    articles = []
    links = []

    for article in soup.find_all("article"):
        title = article.find("h1") or article.find("h2")
        category = article.get("class", [])
        
        comments = None
        possible_comment_classes = ["comments", "comment-count", "article-comments", "comments-number", "komentare", "pocet_komentaru", "clanek-komentare", "komentare-pocet"]
        for comment_class in possible_comment_classes:
            comments_element = article.find("span", class_=comment_class) or article.find("div", class_=comment_class)
            if comments_element:
                try:
                    comments = int(comments_element.get_text(strip=True).split()[0]) 
                except ValueError:
                    comments = 0
                break
        
        if comments is None: 
            comments = 0

        images = article.find_all("img") 
        content = ""
        for paragraph in article.find_all(["p", "div", "span"]):  
            content += paragraph.get_text(separator=" ", strip=False)

        if title:
            articles.append({
                "nadpis": title.get_text(strip=True),
                "kategorie": category,
                "komentare": comments,
                "fotky": len(images),  
                "obsah": content if content else "Neznámý obsah"
            })

    for link in soup.find_all("a"):
        href = link.get("href")
        if href and not href.startswith(("mailto:", "javascript:")):
            if not any(exclude in href for exclude in EXCLUDE_PATHS):
                full_url = href if href.startswith("http") else base_url.rstrip("/") + "/" + href.lstrip("/")
                if is_allowed_domain(full_url):
                    links.append(full_url)
                else:
                    log_message(f"Excluded link: {full_url}") 

    return articles, links

def save_to_json(data, file_path):
    """Safely saves articles to a JSON file."""
    if not data:
        return

    try:
        existing_data = []
        if os.path.exists(file_path):
            with open(file_path, "r", encoding="utf-8") as file:
                try:
                    existing_data = json.load(file)
                except json.JSONDecodeError:
                    log_message(f"Warning: {file_path} is empty or malformed. Starting fresh.")
                    existing_data = []

        new_data = [article for article in data if article not in existing_data]
        if new_data:
            existing_data.extend(new_data)

            with open(file_path, "w", encoding="utf-8") as file:
                json.dump(existing_data, file, ensure_ascii=False, indent=4)

            if os.path.getsize(file_path) >= MAX_FILE_SIZE:
                log_message(f"File size reached ({MAX_FILE_SIZE} bytes). Stopping crawler.")
                return "STOP"
        else:
            log_message("No new data to append.")
    except IOError as e:
        log_message(f"Error saving data to {file_path}: {e}")

def backup_data(file_path):
    """Backup the JSON file periodically to avoid data loss."""
    timestamp = time.strftime("%Y%m%d-%H%M%S")
    backup_path = f"{file_path}_{timestamp}.bak"
    shutil.copy(file_path, backup_path)
    log_message(f"Backup saved as {backup_path}")

async def process_page(url, depth, session):
    """Processes a single page."""
    normalized_url = normalize_url(url)

    if normalized_url in visited_links:
        return

    visited_links.add(normalized_url) 

    html, final_url = await fetch_page(url, session)
    if not html:
        return

    loop = asyncio.get_event_loop()
    articles, links = await loop.run_in_executor(executor, parse_page, html, final_url)

    global collected_articles
    collected_articles += len(articles)

    if save_to_json(articles, DATA_FILE) == "STOP":
        return "STOP"

    if collected_articles >= BACKUP_INTERVAL:
        backup_data(DATA_FILE)
        collected_articles = 0

    if depth < MAX_DEPTH:
        for link in links:
            normalized_link = normalize_url(link)
            if normalized_link not in visited_links and (normalized_link, depth + 1) not in all_links_to_visit:
                all_links_to_visit.add((normalized_link, depth + 1))

async def crawl():
    """Main crawling function."""
    global session
    session = ClientSession()

    try:
        last_backup_time = time.time()

        while all_links_to_visit:
            tasks = []
            for _ in range(min(CONCURRENT_REQUESTS, len(all_links_to_visit))):
                url, depth = all_links_to_visit.pop()
                tasks.append(process_page(url, depth, session))

            results = await asyncio.gather(*tasks, return_exceptions=True)

            if failed_links:
                with open(FAILED_LINKS_FILE, "a", encoding="utf-8") as file:
                    for link in failed_links:
                        file.write(link + "\n")
                log_message(f"Failed links saved after batch. Total failed: {len(failed_links)}.")
                failed_links.clear()

            if time.time() - last_backup_time > BACKUP_TIME_INTERVAL:
                backup_data(DATA_FILE)
                last_backup_time = time.time()

            if "STOP" in results:
                break
    except KeyboardInterrupt:
        log_message("Crawl interrupted.")
    finally:
   
        if failed_links:
            with open(FAILED_LINKS_FILE, "a", encoding="utf-8") as file: 
                for link in failed_links:
                    file.write(link + "\n")
            log_message(f"Failed to fetch {len(failed_links)} URLs. Details saved to '{FAILED_LINKS_FILE}'.")
        
        await session.close()

if __name__ == "__main__":
    try:
        asyncio.run(crawl())
    except KeyboardInterrupt:
        log_message("Crawl interrupted.")
