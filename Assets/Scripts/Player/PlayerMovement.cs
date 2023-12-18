using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 1f;

    private Rigidbody rb;
    private Transform camTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        camTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 movementVectorNormalized = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 movement = new Vector3(movementVectorNormalized.x, 0f, movementVectorNormalized.y);
        movement = camTransform.forward * movement.z + camTransform.right * movement.x;
        movement.y = 0f;
        rb.velocity = movement * movementSpeed;
    }
}
