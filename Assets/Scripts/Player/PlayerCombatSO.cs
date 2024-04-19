using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCombatSO", menuName = "PlayerCombatSO")]
public class PlayerCombatSO : ScriptableObject
{
    [Header("Combat Stats")]
    public Vector3 attackBox;
    public Vector3 superAttackBox;
    public float normalAttackDamage;
    public float superAttackDamage;
    public float attackDistance;
    public float normalAttackDelay;
    public float superAttackDelay;
    public float superAttackStaminaCost;
    public float minimumSuperAttackWindupTime;
    public float healAmountOnKill;

    [Header("Animation")]
    public string defaultSwordPositionAnimation;
    public List<string> normalSwordAttackAnimations;
    public string superAttackWindupAnimation;
    public string superAttackAnimation;
}
