using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class IdleState : EnemyStateBase
{
    public IdleState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy) { }

    public override void OnEnter()
    {
        Debug.Log("ENTER IDLE STATE");
        base.OnEnter();
        agent.isStopped = true;
        animator.Play("Empty");
    }
}
