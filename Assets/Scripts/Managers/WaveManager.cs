using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private float noSpawnRadius = 5f;
    [SerializeField] List<Transform> enemySpawnPoints;
    [SerializeField] List<WaveSO> waves;
    public static event Action<WaveSO> OnNewWave;

    private List<GameObject> enemiesToSpawn;
    private int enemiesRemaining;
    private int waveIndex = 0;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        enemiesToSpawn = new();
        SpawnWave();
    }

    private void SpawnWave()
    {
        OnNewWave?.Invoke(waves[waveIndex]);
        foreach (GameObject gameObject in waves[waveIndex].enemies)
        {
            enemiesToSpawn.Add(gameObject);
        }
        InvokeRepeating(nameof(SpawnEnemy), waves[waveIndex].prepTime, waves[waveIndex].spawnDelay);
    }

    private void SpawnEnemy()
    {
        if (enemiesToSpawn.Count == 0)
        {
            CancelInvoke(nameof(SpawnEnemy));
            return;
        }

        Transform randomSpawnPoint;
        // This loop runs until we find a spawn point that is out of range of
        // noSpawnRadius from player
        while (true)
        {
            var idx = UnityEngine.Random.Range(0, enemySpawnPoints.Count);
            randomSpawnPoint = enemySpawnPoints[idx];
            // Distance from player in xz-plane, we don't care about height
            var dist = Vector2.Distance(
                new Vector2(player.position.x, player.position.z),
                new Vector2(randomSpawnPoint.position.x, randomSpawnPoint.position.z)
            );
            if (dist > noSpawnRadius)
            {
                break;
            }
        }
        GameObject enemy = Instantiate(enemiesToSpawn[0], randomSpawnPoint.position, Quaternion.identity);
        enemy.GetComponent<Health>().OnDeath += Enemy_OnDeath;

        enemiesToSpawn.RemoveAt(0);
        enemiesRemaining++;
    }

    private void Enemy_OnDeath()
    {
        enemiesRemaining--;
        if (enemiesRemaining == 0 && enemiesToSpawn.Count == 0)
        {
            waveIndex++;
            SpawnWave();
        }
    }
}
