using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public event Action<Collider> OnHit;

    [SerializeField] private Vector3 followOffset;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float speedMultiplier = 1.075f;

    private Transform target;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position + followOffset, speed * Time.deltaTime);
        speed *= speedMultiplier;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;

        OnHit?.Invoke(collider);
        Destroy(gameObject);
    }
}
