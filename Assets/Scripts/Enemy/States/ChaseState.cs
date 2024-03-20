using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityHFSM;

public class ChaseState : EnemyStateBase
{
    private EnemyManager enemyManager;

    private Transform target;
    private Transform enemy;
    private Transform enemyFieldOfView;

    private LayerMask targetLayer;

    private int destinationIndex;
    private float playerDetectionDistance = 6f;

    public ChaseState(bool needsExitTime, Enemy enemy, Transform target, Transform enemyFieldOfView) : base(needsExitTime, enemy) 
    {
        this.enemyFieldOfView = enemyFieldOfView;
        this.enemy = enemy.transform;
        this.target = target;
        enemyManager = EnemyManager.Instance;
        destinationIndex = enemyManager.GetRandomDestinationIndex();
        targetLayer = target.gameObject.layer;
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
            Vector3 enemyPlanePosition = new Vector3(enemy.position.x, 0, enemy.position.z);
            Vector3 targetPlanePosition = new Vector3(target.position.x, 0, target.position.z);

            Ray ray = new Ray(enemyFieldOfView.position, (targetPlanePosition + Vector3.up) - (enemyPlanePosition + Vector3.up));
            Debug.DrawRay(enemyFieldOfView.position, (targetPlanePosition + Vector3.up) - (enemyPlanePosition + Vector3.up), Color.magenta);


            bool hasLineOfSight = false;
            if (Physics.Raycast(ray, out RaycastHit hitInfo, playerDetectionDistance))
            {
                Debug.Log(hitInfo.collider.name);
                if (hitInfo.collider.CompareTag("Player")) {
                    hasLineOfSight = true;
                }
            }


            if (Vector3.Distance(enemyPlanePosition, targetPlanePosition) > playerDetectionDistance || !hasLineOfSight)
            {
                agent.SetDestination(enemyManager.GetDestinationAroundPlayer(destinationIndex));
            }
            else
            {
                agent.SetDestination(target.position);
            }
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            fsm.StateCanExit();
        }
    }
}
