using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private float noSpawnRadius = 5f;
    [SerializeField] List<Transform> enemySpawnPoints;
    [SerializeField] List<WaveSO> waves;
    [SerializeField] private float wavesCompleteteRewardAmount;
    public static event Action<WaveSO> OnNewWave;
    public static event Action OnWavesCompleted;

    private List<EnemySpawn> enemiesToSpawn;
    private int enemiesRemaining;
    private int waveIndex = 0;
    private Transform player;
    private PlayerBalance playerBalance;
    private float timeSinceLastEnemy;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerBalance = player.GetComponent<PlayerBalance>();
        enemiesToSpawn = new();
    }

    private void Update()
    {
        if (enemiesToSpawn.Count > 0 && timeSinceLastEnemy < Time.time - enemiesToSpawn[0].delay)
        {
            SpawnEnemy();
        }
    }

    private void SpawnWave()
    {
        OnNewWave?.Invoke(waves[waveIndex]);
        foreach (EnemySpawn enemySpawn in waves[waveIndex].enemies)
        {
            enemiesToSpawn.Add(enemySpawn);
        }
    }

    private void SpawnEnemy()
    {
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
        GameObject enemy = Instantiate(enemiesToSpawn[0].enemyType, randomSpawnPoint.position, Quaternion.identity);
        enemy.GetComponent<Health>().OnDeath += Enemy_OnDeath;

        enemiesRemaining++;
        enemiesToSpawn.RemoveAt(0);
        timeSinceLastEnemy = Time.time;
    }

    private void Enemy_OnDeath()
    {
        enemiesRemaining--;
        CheckRemainingEnemies();
    }

    private void CheckRemainingEnemies()
    {
        if (enemiesRemaining == 0 && enemiesToSpawn.Count == 0)
        {
            if (AreWavesCompleted())
            {
                OnWavesCompleted?.Invoke();
                Debug.Log("ADD TO PLAYER MONEY: " + wavesCompleteteRewardAmount);
                SaveData.Instance.data.playerBalance += wavesCompleteteRewardAmount;
            }
            else
            {
                waveIndex++;
                SpawnWave();
            }
        }
    }

    private bool AreWavesCompleted()
    {
        return waveIndex == waves.Count - 1;
    }

    public void StartWaves(float seconds)
    {
        Invoke(nameof(SpawnWave), seconds);
    }
}
