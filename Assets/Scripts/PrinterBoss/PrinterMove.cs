using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrinterMove : StateMachineBehaviour
{
    [SerializeField] private float distanceToAttack = 10;

    private Transform transform;
    private GameObject target;
    private NavMeshAgent navMeshAgent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transform = animator.transform;

        target = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = animator.GetComponent<NavMeshAgent>();

        navMeshAgent.enabled = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent.enabled = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetDestinationToTarget();
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= distanceToAttack)
        {
            animator.SetTrigger("ChooseAttack");
        }
    }

    private void SetDestinationToTarget()
    {
        navMeshAgent.SetDestination(target.transform.position);
    }
}
