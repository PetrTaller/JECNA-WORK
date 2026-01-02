using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public int currentEnemies;
    public int maxEnemies = 5;

    void Update()
    {
        if (currentEnemies < maxEnemies)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        currentEnemies++;
        Transform spawnpoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnpoint.position, spawnpoint.rotation);
    }
    public void EnemyDestroy()
    {
        currentEnemies --;
    }
    
}
