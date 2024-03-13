using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private Health health;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        SetDestinationToPlayerPosition();
    }

    private void SetDestinationToPlayerPosition()
    {
        navMeshAgent.destination = player.position;
    }
}
