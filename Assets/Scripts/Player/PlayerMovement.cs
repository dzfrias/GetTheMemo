using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    private float movementSpeed = 1f;

    private Transform camTransform;
    private CharacterController characterController;

    private void Awake()
    {
        camTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        RotateToCamera();
        ApplyGravity();
    }

    private void Move()
    {
        Vector2 movementVectorNormalized = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 movement = new Vector3(movementVectorNormalized.x, 0f, movementVectorNormalized.y);
        movement = camTransform.forward * movement.z + camTransform.right * movement.x;
        movement.y = 0f;
        characterController.Move(movement * movementSpeed * Time.deltaTime);
    }

    private void RotateToCamera()
    {
        transform.rotation = Quaternion.Euler(0f, camTransform.rotation.eulerAngles.y, 0f);
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded)
            return;

        characterController.Move(Physics.gravity * Time.deltaTime);
    }
}
