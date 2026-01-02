using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth = 100;
    private EnemyHealthBar healthBar;
    private EnemySpawner spawner;
    public Material destroyedMaterial;

    private void Start()
    {
        healthBar = GetComponent<EnemyHealthBar>();
        currentHealth = maxHealth;
        healthBar.updateHealthBar(currentHealth, maxHealth);
        spawner = FindObjectOfType<EnemySpawner>();
    }

    public void TakeDamage(float damage )
    {
        currentHealth -= damage;
        healthBar.updateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 5)
        {
            this.GetComponent<Renderer>().material = destroyedMaterial;
        }

        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
            spawner.EnemyDestroy();
        }

    }

}
