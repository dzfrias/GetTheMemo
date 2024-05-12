using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityHFSM;

public class FleeState : EnemyStateBase
{
    private Transform target;
    private Transform enemy;

    public FleeState(bool needsExitTime, Enemy enemy, Transform target) : base(needsExitTime, enemy) 
    {;
        this.enemy = enemy.transform;
        this.target = target;
    }

    public override void OnEnter()
    {
        Debug.Log("ENTER CHASE STATE");
        base.OnEnter();
        agent.enabled = true;
        agent.isStopped = false;
        animator.Play("Walk");
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (!requestedExit)
        {
            Vector3 directionTowardsTarget = enemy.transform.position - target.position;

            Vector3 newPosition = enemy.position + directionTowardsTarget;
            agent.SetDestination(newPosition);
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            fsm.StateCanExit();
        }
    }
}
