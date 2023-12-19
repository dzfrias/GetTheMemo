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
    public event Action OnPickup;
    public event Action OnDrop;

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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        playerInputActions.Player.Interact.performed += PlayerInputActions_OnInteract;
        playerInputActions.Player.Pickup.started += PlayerInputActions_OnPickupStarted;
        playerInputActions.Player.Pickup.canceled += PlayerInputActions_OnPickupStopped;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Interact.performed -= PlayerInputActions_OnInteract;
        playerInputActions.Player.Pickup.started -= PlayerInputActions_OnPickupStarted;
        playerInputActions.Player.Pickup.canceled -= PlayerInputActions_OnPickupStopped;
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

    private void PlayerInputActions_OnPickupStarted(InputAction.CallbackContext _)
    {
        OnPickup?.Invoke();
    }

    private void PlayerInputActions_OnPickupStopped(InputAction.CallbackContext _)
    {
        OnDrop?.Invoke();
    }
}