using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

[RequireComponent(typeof(NavMeshAgent))]
public class SupportEnemy : Enemy
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float stimulateTimerMax;

    private float stimulateTimer;

    protected override void Awake()
    {
        base.Awake();
        stimulateTimer = stimulateTimerMax;
    }

    protected override void Update()
    {   
        base.Update();
        stimulateTimer -= Time.deltaTime;
    }

    public override void AddStatesToEnemyFSM()
    {
        base.AddStatesToEnemyFSM();
        enemyFSM.AddState(EnemyState.Attack, new AttackState(true, this, OnAttack, 1f));
    }

    public override void AddEnemyStateTransitions()
    {
        base.AddEnemyStateTransitions();
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, CanAttack));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, CanAttack));
    }

    private bool CanAttack(Transition<EnemyState> _)
    {
        if (stimulateTimer <= 0)
        {
            stimulateTimer = stimulateTimerMax;
            return true;
        }
        return false;
    }

    protected override void PrimaryAttack()
    {
        StimulateAllies();
    }

    private void StimulateAllies()
    {
        Collider[] hitColliders = Physics.OverlapBox(attackPoint.position, enemySO.attackBox/2, transform.rotation);
        foreach (Collider collider in hitColliders)
        {
            if (IsEnemyLayer(collider.gameObject.layer) && collider.gameObject != gameObject)
            {
                Debug.Log("Boost Enemy: " + collider.gameObject.name);
            }
        }
    }
    
    private bool IsEnemyLayer(int layer)
    {
        return (enemyLayer & (1 << layer)) != 0;
    }
}
