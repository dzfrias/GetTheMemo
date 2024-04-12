using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "EnemySO")]
public class EnemySO : ScriptableObject
{
    public float attackDamage;
    public float attackCooldown;
    public float maxHealth;
    public float attackDistance;
}
