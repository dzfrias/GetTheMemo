using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class ChaseState : EnemyStateBase
{
    private Transform target;

    public ChaseState(bool needsExitTime, Enemy enemy, Transform target) : base(needsExitTime, enemy) 
    {
        this.target = target;
    }

    public override void OnEnter()
    {
        Debug.Log("ENTER CHASE STATE");
        base.OnEnter();
        agent.enabled = true;
        agent.isStopped = false;
        animator.Play("Empty");
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (!requestedExit)
        {
            agent.SetDestination(target.position);
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            fsm.StateCanExit();
        }
    }
}
