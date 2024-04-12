using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSO")]
public class WaveSO : ScriptableObject
{
    public List<GameObject> enemies;
    public float prepTime;
    public float spawnDelay;
    public int lengthHours = 1;
}
