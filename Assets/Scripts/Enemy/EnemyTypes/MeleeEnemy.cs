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
        if (!IsInMeleeRange()) return false;

        if (isInitialAttack && initialAttackWaitTime > 0)
        {
            initialAttackWaitTime -= Time.deltaTime;
            return false;
        }

        return lastAttackTime + enemySO.attackCooldown < Time.time;
    }

    private bool IsInMeleeRange()
    {
        return Vector3.Distance(player.transform.position, transform.position) <= navMeshAgent.stoppingDistance;
    }
}
