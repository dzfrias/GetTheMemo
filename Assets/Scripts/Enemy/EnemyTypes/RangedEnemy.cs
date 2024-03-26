using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class RangedEnemy : Enemy
{
    public override void AddStatesToEnemyFSM()
    {
        base.AddStatesToEnemyFSM();
        enemyFSM.AddState(EnemyState.Attack, new AttackState(true, this, ShootArrow, exitTime: 4f));
    }

    private void ShootArrow(State<EnemyState, StateEvent> state)
    {
        lastAttackTime = Time.time;
        Debug.Log("SHOOT PROJECTILE");
    }

    public override void AddEnemyStateTransitions()
    {
        base.AddEnemyStateTransitions();
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, ShouldShoot));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldShoot));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Impact, EnemyState.Attack, ShouldShoot));
    }

    private bool ShouldShoot(Transition<EnemyState> _)
    {
        return lastAttackTime + attackCooldown < Time.time && IsInShootingRange();
    }

    private bool IsInShootingRange()
    {
        return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    }
}
