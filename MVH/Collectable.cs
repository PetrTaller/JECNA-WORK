using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Collectable : MonoBehaviour
{
    public GameObject prompt;
    public TextMeshProUGUI hintText;
    public GameObject bridge;


    private bool playerInside = false;

    private void Start()
    {
        prompt.SetActive(false);
    }

    private void Update()
    {
        if (playerInside == true && Input.GetKeyDown(KeyCode.E))
        {
            bridge.SetActive(true);
            hintText.text = "";
            prompt.SetActive(false);
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            prompt.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            prompt.SetActive(false);
        }
    }

}
