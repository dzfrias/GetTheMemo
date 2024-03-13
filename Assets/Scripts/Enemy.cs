using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector3 attackArea;
    [SerializeField] private float attackDelay = 0.5f;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float damage = 2f;

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private Health health;

    private bool isAttacking = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetDestinationToPlayerPosition();
    }

    private void OnEnable()
    {
        health.OnHealthChanged += Health_OnHealthChanged;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= Health_OnHealthChanged;
    }

    private void Health_OnHealthChanged(float health)
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!isAttacking)
        {
            SetDestinationToPlayerPosition();
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        Debug.Log("ENEMY ATTACK");
        isAttacking = true;
        Invoke(nameof(HitArea), attackDelay);
    }

    private void HitArea()
    {
        Debug.Log("ENEMY TRY HIT");
        Collider[] hitColliders = Physics.OverlapBox(attackTransform.position, attackArea/2, Quaternion.identity);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                hitCollider.GetComponent<Health>().TakeDamage(damage);
                Debug.Log("ENEMY HIT");
            }
        }
        isAttacking = false;
    }

    private void SetDestinationToPlayerPosition()
    {
        navMeshAgent.destination = player.position;
    }
}
