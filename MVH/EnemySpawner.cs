using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public int maxEnemies = 3;
    public int currentEnemies = 0;

    void Update()
    {
        if (currentEnemies < maxEnemies)
        {
            SpawnEnemy();
        }
    }


    public void SpawnEnemy()
    {
        currentEnemies++;
        Transform spawnpoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnpoint.position, spawnpoint.rotation);
    }


    public void EnemyDestroyed()
    {
        currentEnemies--;
    }
}
