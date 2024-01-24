using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrinterBoss : MonoBehaviour
{
    [SerializeField] private GameObject paperProjectilePrefab;

    private NavMeshAgent navMeshAgent;
    private GameObject target;

    private State state;
    private float attackTime = 3f;

    private float maxXVariance = 1f;
    private float maxYVariance = 1f;

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
        PaperAttack();
        yield return new WaitForSeconds(attackTime);
        SwitchState(State.Move);
    }

    private void SetDestinationToTarget()
    {
        navMeshAgent.SetDestination(target.transform.position);
    }

    private void PaperAttack()
    {
        StartCoroutine(ShootPapers(5, 0.25f));
    }

    private IEnumerator ShootPapers(int amount, float timeBetweenAttacks)
    {
        for (int i = 0; i < amount; i++)
        {
            ShootPaper();
            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    private void ShootPaper()
    {
        GameObject paper = Instantiate(paperProjectilePrefab, transform.position, transform.rotation);
        PaperProjectile paperProjectile = paper.GetComponent<PaperProjectile>();
        paperProjectile.SetTargetShootingPosition(GetTargetShootingPosition());
    }

    private Vector3 GetTargetShootingPosition()
    {
        float targetHeight = target.GetComponent<CharacterController>().height;
        Vector3 targetShootingPosition = target.transform.position + Vector3.up * targetHeight/2 + GetTargetVariance();
        return targetShootingPosition;
    }

    private Vector3 GetTargetVariance()
    {
        float xVariance = Random.Range(0, maxXVariance);
        float yVariance = Random.Range(0, maxYVariance);
        Vector3 varientVector = new Vector3 (xVariance, yVariance, 0f);
        return varientVector;
    }
}
