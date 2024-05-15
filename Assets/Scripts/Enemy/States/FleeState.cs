using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

public class FleeState : EnemyStateBase
{
    private Transform target;
    private Transform enemy;
    private List<Transform> fleeLocations;
    private float normalStoppingDistance;
    private float fleeingStoppingDistance = 0.1f;

    public FleeState(bool needsExitTime, Enemy enemy, Transform target, List<Transform> fleeLocations) : base(needsExitTime, enemy) 
    {;
        this.enemy = enemy.transform;
        this.target = target;
        this.fleeLocations = fleeLocations;
    }

    public override void OnEnter()
    {
        Debug.Log("ENTER FLEE STATE");
        base.OnEnter();
        agent.enabled = true;
        agent.isStopped = false;
        animator.Play("Walk");
        normalStoppingDistance = agent.stoppingDistance;
        agent.stoppingDistance = fleeingStoppingDistance;
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (!requestedExit)
        {
            Transform furthestPoint = null;
            foreach (Transform fleeLocation in fleeLocations)
            {
                NavMeshPath navMeshPath = new NavMeshPath();
                if (agent.CalculatePath(fleeLocation.position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    if (furthestPoint == null || Vector3.Distance(target.position, fleeLocation.position) > Vector3.Distance(target.position, furthestPoint.position))
                    {
                        furthestPoint = fleeLocation;
                    }
                }
            }

            agent.SetDestination(furthestPoint.position);
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            fsm.StateCanExit();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        agent.stoppingDistance = normalStoppingDistance;
    }
}
