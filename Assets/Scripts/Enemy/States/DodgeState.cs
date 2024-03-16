using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class DodgeState : EnemyStateBase
{
    [SerializeField] private float moveAmount = 1f;

    public DodgeState(
        bool needsExitTime,
        Enemy enemy,
        float exitTime = 0.1F,
        Action<State<EnemyState, StateEvent>> onEnter = null,
        Action<State<EnemyState, StateEvent>> onLogic = null,
        Func<State<EnemyState, StateEvent>, bool> canExit = null) : base(needsExitTime, enemy, exitTime, onEnter, onLogic, canExit) { }

    public override void OnEnter()
    {
        base.OnEnter();
        agent.isStopped = true;
        agent.Move(agent.transform.forward * -moveAmount);
    }
}
