using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityHFSM;

public class FollowState : EnemyStateBase
{
    private Transform target;
    private Transform enemy;

    public FollowState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy) 
    {
        this.enemy = enemy.transform;
    }

    public override void OnEnter()
    {
        Debug.Log("ENTER FOLLOW STATE");
        base.OnEnter();
        agent.enabled = true;
        agent.isStopped = false;
        animator.Play("Walk");
    }

    private Transform FindClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapBox(enemy.position, new Vector3(100, 100, 100), Quaternion.identity);
        Transform closestEnemy = null;
        foreach (Collider collider in hitColliders)
        {
            if (!collider.CompareTag("Enemy") || collider.TryGetComponent(out SupportEnemy _)) continue;

            if (closestEnemy == null)
            {
                closestEnemy = collider.transform;
            }
            else if (Vector3.Distance(collider.transform.position, enemy.position) < Vector3.Distance(closestEnemy.position, enemy.position))
            {
                closestEnemy = collider.transform;
            }
        }
        return closestEnemy;
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (!requestedExit)
        {
            Transform closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                agent.SetDestination(closestEnemy.position);
            }
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            fsm.StateCanExit();
        }
    }
}
