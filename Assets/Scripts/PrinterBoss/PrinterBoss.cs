using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrinterBoss : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private GameObject target;

    private State state;
    private float attackTime = 3f;

    private enum State
    {
        Move,
        Attack
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        SwitchState(State.Move);
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (state == State.Move)
        {
            SetDestinationToTarget();
            Debug.Log(navMeshAgent.remainingDistance);
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                SwitchState(State.Attack);
            }
        }
    }

    private void SwitchState(State state)
    {
        this.state = state;
        switch(state)
        {
            case State.Move:
                break;
            case State.Attack:
                StartCoroutine(Attack());
                break;
        }
    }

    private IEnumerator Attack()
    {
        animator.SetTrigger("PaperAttack");
        Debug.Log("Trigger Paper Attack");
        yield return new WaitForSeconds(attackTime);
        SwitchState(State.Move);
    }

    private void SetDestinationToTarget()
    {
        navMeshAgent.SetDestination(target.transform.position);
    }
}
