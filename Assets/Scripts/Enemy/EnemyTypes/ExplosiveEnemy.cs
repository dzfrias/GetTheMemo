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
        enemyFSM.AddTriggerTransitionFromAny(StateEvent.Death, EnemyState.Death, forceInstantly: true);
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, IsInExplosionRange));
    }

    private bool IsInExplosionRange(Transition<EnemyState> _)
    {
        return Vector3.Distance(player.position, transform.position) < navMeshAgent.stoppingDistance;
    }

    protected override void PrimaryAttack()
    {
        Debug.Log("SHOULD EXPLODE");
        base.PrimaryAttack();
        Explode();
    }

    private void Explode()
    {
        Debug.Log("EXPLODE!!!");
        Instantiate(explosion, transform.position, Quaternion.identity);
        StopAllCoroutines();
        health.TakeDamage(Mathf.Infinity, transform.position);
        Destroy(gameObject);
    }
}
