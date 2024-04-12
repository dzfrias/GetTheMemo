using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class DeathState : EnemyStateBase
{
    public DeathState(
        bool needsExitTime,
        Enemy enemy,
        Action<State<EnemyState, StateEvent>> onEnter = null,
        float exitTime = 2f) : base(needsExitTime, enemy, exitTime, onEnter) { }

    public override void OnEnter()
    {
        Debug.Log("ENTER DEATH STATE");
        base.OnEnter();
        agent.isStopped = true;
        if (agent.TryGetComponent(out Collider collider))
        {
            collider.enabled = false;
        }
        animator.Play("Death");
    }
}
