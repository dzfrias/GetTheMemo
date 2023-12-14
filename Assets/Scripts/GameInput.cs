using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;

    public event Action OnInteract;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There is more than 1 GameInput in the scene");
        }

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Interact.performed += PlayerInputActions_OnInteract;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Interact.performed -= PlayerInputActions_OnInteract;
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 movementVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        movementVector = movementVector.normalized;
        return movementVector;
    }

    public Vector2 GetMouseMovement()
    {
        Vector2 mouseVector = playerInputActions.Player.Look.ReadValue<Vector2>();
        return mouseVector;
    }

    private void PlayerInputActions_OnInteract(InputAction.CallbackContext _)
    {
        OnInteract?.Invoke();
    }
}
