using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCombatSO", menuName = "PlayerCombatSO")]
public class PlayerCombatSO : ScriptableObject
{
    public Vector3 attackBox;
    public Vector3 superAttackBox;
    public float normalAttackDamage;
    public float superAttackDamage;
    public float attackDistance;
    public float normalAttackDelay;
    public float superAttackDelay;
    public float superAttackStaminaCost;
    public float healAmountOnKill;
    public float staminaIncreaseAmountOnKill;

    public List<string> normalSwordAttackAnimations;
    public string superAttackWindupAnimation;
    public string superAttackAnimation;
}
