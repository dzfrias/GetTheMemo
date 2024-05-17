using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSO")]
public class WaveSO : ScriptableObject
{
    public List<EnemySpawn> enemies;
    public float prepTime;
    public int lengthHours = 1;
}

[Serializable]
public struct EnemySpawn
{
    public GameObject enemyType;
    public float delay;
}
