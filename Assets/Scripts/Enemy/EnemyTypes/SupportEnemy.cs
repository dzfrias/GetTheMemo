using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        enemyFSM.AddState(EnemyState.Idle, new IdleState(false, this));
        enemyFSM.AddState(EnemyState.Follow, new FollowState(false, this));
        enemyFSM.AddState(EnemyState.Attack, new AttackState(true, this, OnAttack, 1f));
        enemyFSM.AddState(EnemyState.Flee, new FleeState(false, this, player));
        enemyFSM.AddState(EnemyState.Death, new DeathState(false, this));

        enemyFSM.SetStartState(EnemyState.Flee);
    }

    public override void AddEnemyStateTransitions()
    {
        enemyFSM.AddTwoWayTransition(EnemyState.Flee, EnemyState.Follow, CanFollow);
        //enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Follow, EnemyState.Attack, CanAttack));
        //enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Follow, CanFollow));
        //enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Follow, EnemyState.Idle, ShouldIdle));
        //enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Follow, EnemyState.Flee, ShouldFlee));
        //enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Flee, ShouldFlee));

        enemyFSM.AddTriggerTransitionFromAny(StateEvent.Death, EnemyState.Death, forceInstantly: true);
    }

    private bool ShouldFlee(Transition<EnemyState> _)
    {
        return GetEnemyAmount() == 0;
    }

    private bool CanFollow(Transition<EnemyState> _)
    {
        Debug.Log("CHECKING ENEMY AMOUNT: " + GetEnemyAmount());
        return GetEnemyAmount() > 0;
    }

    private bool ShouldIdle(Transition<EnemyState> _)
    {
        return GetEnemyAmount() > 0 && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    }

    private int GetEnemyAmount()
    {
        int count = 0;
        Collider[] hitColliders = Physics.OverlapBox(transform.position, new Vector3(100, 100, 100), Quaternion.identity);
        foreach (Collider hitCollider in hitColliders)
        {
            if (!hitCollider.CompareTag("Enemy") || hitCollider.TryGetComponent(out SupportEnemy _)) continue;
            count++;
        }
        return count;
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
