using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

[RequireComponent(typeof(NavMeshAgent))]
public class MeleeEnemy : Enemy
{
    public override void AddStatesToEnemyFSM()
    {
        base.AddStatesToEnemyFSM();
        enemyFSM.AddState(EnemyState.Attack, new AttackState(true, this, OnAttack, 1f));
    }

    public override void AddEnemyStateTransitions()
    {
        base.AddEnemyStateTransitions();
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, ShouldMelee));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldMelee));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Impact, EnemyState.Attack, ShouldMelee));
    }

    private bool ShouldMelee(Transition<EnemyState> _)
    {
        return lastAttackTime + enemySO.attackCooldown < Time.time && IsInMeleeRange();
    }

    private bool IsInMeleeRange()
    {
        return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    }
}
