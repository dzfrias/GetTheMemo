using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouseLook : MonoBehaviour
{
    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensX = 1f;
    [SerializeField] private float mouseSensY = 1f;

    private float xRotation;
    private float yRotation;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        xRotation = transform.rotation.x;
        yRotation = transform.rotation.y;
    }

    private void Update()
    {
        MouseMovement();
    }

    private void MouseMovement()
    {
        Vector2 mouseInput = GameInput.Instance.GetMouseMovement();
        xRotation -= mouseInput.y * Time.deltaTime * mouseSensX;
        yRotation += mouseInput.x * Time.deltaTime * mouseSensY;
        transform.rotation = Quaternion.Euler(xRotation, yRotation, transform.rotation.z);
    }
}
