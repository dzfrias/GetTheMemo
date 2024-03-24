using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class ImpactState : EnemyStateBase
{
    public ImpactState(
        bool needsExitTime,
        Enemy enemy,
        Action<State<EnemyState, StateEvent>> onEnter = null,
        float exitTime = 1f) : base(needsExitTime, enemy, exitTime, onEnter) { }

    public override void OnEnter()
    {
        Debug.Log("ENTER IMPACT STATE");
        base.OnEnter();
        agent.isStopped = true;
        animator.Play("Impact");
    }
}
