using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] List<Transform> enemySpawnPoints;
    [SerializeField] List<WaveSO> waves;
    public static event Action<WaveSO> OnNewWave;

    private List<GameObject> enemiesToSpawn;
    private int enemiesRemaining;
    private int waveIndex = 0;
    private bool isSpawning = false;

    private void Start()
    {
        enemiesToSpawn = new();
        SpawnWave();
    }

    private void Update()
    {
        if (enemiesRemaining <= 0 && !isSpawning && waveIndex < waves.Count)
        {
            waveIndex++;
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        OnNewWave?.Invoke(waves[waveIndex]);
        isSpawning = true;
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
            isSpawning = false;
            return;
        }

        int randomSpawnIndex = UnityEngine.Random.Range(0, enemySpawnPoints.Count);
        Transform randomSpawnPoint = enemySpawnPoints[randomSpawnIndex];
        GameObject enemy = Instantiate(enemiesToSpawn[0], randomSpawnPoint.position, Quaternion.identity);
        enemy.GetComponent<Health>().OnDeath += Enemy_OnDeath;

        enemiesToSpawn.RemoveAt(0);
        enemiesRemaining++;
    }

    private void Enemy_OnDeath()
    {
        enemiesRemaining--;
    }
}
