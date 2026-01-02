import json
import random

def modify_nadpis(nadpis):
    nadpis_words = nadpis.split()
    # Randomly remove or add words to the nadpis (title)
    if len(nadpis_words) > 3 and random.random() < 0.5:
        # Remove a random word
        nadpis_words.pop(random.randint(0, len(nadpis_words) - 1))
    elif random.random() < 0.5:
        # Add a random word
        nadpis_words.insert(random.randint(0, len(nadpis_words)), random.choice(['nový', 'zajímavý', 'neobvyklý', 'neznámý']))
    return " ".join(nadpis_words)

def modify_obsah(obsah):
    obsah_words = obsah.split()
    # Randomly remove, replace, or add words to the obsah (content)
    if len(obsah_words) > 5 and random.random() < 0.5:
        # Remove a random word
        obsah_words.pop(random.randint(0, len(obsah_words) - 1))
    elif random.random() < 0.5:
        # Replace a word with a synonym or a variation
        random_index = random.randint(0, len(obsah_words) - 1)
        obsah_words[random_index] = random.choice(['pokračuje', 'vzhledem', 'dodává', 'říká'])
    else:
        # Add a random word
        obsah_words.append(random.choice(['nové', 'zajímavé', 'důležité', 'pokračování']))
    
    return " ".join(obsah_words)

def modify_entry(entry):
    # Modify nadpis and obsah to make them more varied
    entry['nadpis'] = modify_nadpis(entry['nadpis'])
    entry['obsah'] = modify_obsah(entry['obsah'])
    return entry

def double_json_data(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        data = json.load(f)

    # Create a modified copy of the data
    new_data = []
    for entry in data:
        new_data.append(entry)  # Add the original entry
        new_data.append(modify_entry(entry.copy()))  # Add the modified entry

    # Save the modified data back to the file (or a new file if preferred)
    with open(file_path, 'w', encoding='utf-8') as f:
        json.dump(new_data, f, ensure_ascii=False, indent=4)

# Example usage
double_json_data('merged_data.json')
