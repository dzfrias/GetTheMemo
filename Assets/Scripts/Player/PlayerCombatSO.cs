using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCombatSO", menuName = "PlayerCombatSO")]
public class PlayerCombatSO : ScriptableObject
{
    public float damage;
    public float attackDistance;
    public float attackDelay;
    public float healAmountOnKill;
    public float staminaIncreaseAmountOnKill;

    public List<string> swordAnimations;
}
