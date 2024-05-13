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
    [SerializeField] private float stimDistance = 4f;
    [SerializeField] private float stimulateTime = 5f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {   
        base.Update();
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
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Follow, EnemyState.Attack, ShouldStimEnemies));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Flee, EnemyState.Attack, ShouldStimEnemies));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldStimEnemies));

        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Follow, EnemyState.Idle, ShouldIdle));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Flee, EnemyState.Idle, ShouldIdle));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, ShouldIdle));

        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Follow, ShouldFollow));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Flee, EnemyState.Follow, ShouldFollow));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Follow, ShouldFollow));  

        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Flee, ShouldFlee));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Follow, EnemyState.Flee, ShouldFlee));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Flee, ShouldFlee));  

        enemyFSM.AddTriggerTransitionFromAny(StateEvent.Death, EnemyState.Death, forceInstantly: true);
    }

    private bool AreEnemies()
    {
        return GetEnemyAmount() > 0;
    }

    private Transform GetClosestEnemy()
    {
        Transform closestEnemy = null;
        Collider[] hitColliders = Physics.OverlapBox(transform.position, new Vector3(100, 100, 100), Quaternion.identity);
        foreach (Collider hitCollider in hitColliders)
        {
            if (!hitCollider.CompareTag("Enemy") || hitCollider.TryGetComponent(out SupportEnemy _)) continue;
            if (closestEnemy == null)
            {
                closestEnemy = hitCollider.transform;
            }
            else if (Vector3.Distance(transform.position, hitCollider.transform.position) < Vector3.Distance(transform.position, closestEnemy.position))
            {
                closestEnemy = hitCollider.transform;
            }
        }
        return closestEnemy;
    }

    private bool IsAttackOnCooldown()
    {
        return lastAttackTime + stimulateTimerMax < Time.time;
    }

    private bool ShouldStimEnemies(Transition<EnemyState> _)
    {
        if (!AreEnemies()) return false;
        Transform closestEnemy = GetClosestEnemy();
        return Vector3.Distance(transform.position, closestEnemy.position) < stimDistance && !IsAttackOnCooldown();
    }

    private bool ShouldIdle(Transition<EnemyState> _)
    {
        if (!AreEnemies()) return false;
        Transform closestEnemy = GetClosestEnemy();
        return Vector3.Distance(transform.position, closestEnemy.position) <= navMeshAgent.stoppingDistance && IsAttackOnCooldown();
    } 

    private bool ShouldFollow(Transition<EnemyState> _)
    {
        if (!AreEnemies()) return false;
        Transform closestEnemy = GetClosestEnemy();
        return Vector3.Distance(transform.position, closestEnemy.position) > navMeshAgent.stoppingDistance;
    }

    private bool ShouldFlee(Transition<EnemyState> _)
    {
        return !AreEnemies();
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

    public override void OnAttack(State<EnemyState, StateEvent> _)
    {
        lastAttackTime = Time.time;
        StimulateAllies();
    }

    private void StimulateAllies()
    {
        Collider[] hitColliders = Physics.OverlapBox(attackPoint.position, enemySO.attackBox/2, transform.rotation);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy") && !collider.TryGetComponent(out SupportEnemy _))
            {
                Debug.Log("Boost Enemy: " + collider.gameObject.name);
                Enemy enemy = collider.GetComponent<Enemy>();
                if (!enemy.IsStimulated())
                {
                    StartCoroutine(enemy.Stimulate(stimulateTime));
                }
            }
        }
    }
}
