using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float  health;
    public Slider healthSlider; 
    // Start is called before the first frame update

    private void Start()
    {
        health = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void takeDamage(float amount)
    {
        health -= amount;
        Debug.LogError(health);
        healthSlider.value = health/maxHealth;
    }
}
