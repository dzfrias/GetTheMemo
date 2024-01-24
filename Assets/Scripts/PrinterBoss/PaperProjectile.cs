using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 4f;
    [SerializeField] private float speed;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetTargetShootingPosition(Vector3 targetShootingPosition)
    {
        transform.LookAt(targetShootingPosition);
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }
}
