using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

[RequireComponent(typeof(NavMeshAgent))]
public class ExplosiveEnemy : Enemy
{
    [SerializeField] private GameObject explosion;

    public override void AddStatesToEnemyFSM()
    {
        base.AddStatesToEnemyFSM();
        enemyFSM.AddState(EnemyState.Attack, new AttackState(true, this, OnAttack, 1f));
    }

    public override void AddEnemyStateTransitions()
    {
        base.AddEnemyStateTransitions();
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, IsInExplosionRange));
    }

    private bool IsInExplosionRange(Transition<EnemyState> _)
    {
        return Vector3.Distance(player.position, transform.position) < navMeshAgent.stoppingDistance;
    }

    protected override void PrimaryAttack()
    {
        base.PrimaryAttack();
        Explode();
    }

    private void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        StopAllCoroutines();
        health.TakeDamage(Mathf.Infinity, transform.position);
        Destroy(gameObject);
    }
}
