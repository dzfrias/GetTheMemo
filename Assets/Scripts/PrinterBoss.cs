using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrinterBoss : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    private State state;
    private float attackTime = 5f;

    private enum State
    {
        Move,
        Attack
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        SwitchState(State.Move);
    }

    private void Update()
    {
        if (state == State.Move)
        {
            Debug.Log("MOVING");
            SetDestinationToPlayer();
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
                Debug.Log("ATTACK");
                StartCoroutine(Attack());
                break;
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackTime);
        SwitchState(State.Move);
    }

    private void SetDestinationToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent.SetDestination(player.transform.position);
        Debug.Log("Set Destination to: " + player.transform.position);
    }
}
