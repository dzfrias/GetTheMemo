using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class AttackState : EnemyStateBase
{
    public AttackState(
        bool needsExitTime,
        Enemy enemy,
        Action<State<EnemyState, StateEvent>> onEnter = null,
        float exitTime = 0.33f) : base(needsExitTime, enemy, exitTime, onEnter) { }

    public override void OnEnter()
    {
        var name = enemy.gameObject.name;
        Debug.Log($"{name}: ENTER ATTACK STATE");
        base.OnEnter();
        agent.isStopped = true;
        animator.Play("Attack");
    }
}
