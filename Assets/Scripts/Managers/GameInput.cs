using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum ActionMap
{
    Player,
    PlayerNightTime,
    UI,
    Printer,
    OpeningSequence,
}

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;

    // Player action map
    public event Action OnInteract;
    public event Action OnPickup;
    public event Action OnDrop;
    public event Action OnThrow;
    public event Action OnJump;
    public event Action OnSprintStart;
    public event Action OnSprintStop;
    public event Action OnAttack;
    public event Action OnClick;
    public event Action OnDash;
    public event Action OnPause;

    // UI action map
    public event Action OnOpenUI;
    public event Action OnCloseUI;

    // Printer action map
    public event Action OnPrinterLeft;
    public event Action OnPrinterRight;
    public event Action OnPrinterTop;
    public event Action OnPrinterBottom;

    // Opening sequence action map
    public event Action OnAwake;

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
        LockCursor();
    }

    public void SwitchActionMaps(ActionMap actionMap)
    {
        playerInputActions.Printer.Disable();
        playerInputActions.Player.Disable();
        playerInputActions.PlayerNightTime.Disable();
        playerInputActions.UI.Disable();
        playerInputActions.OpeningSequence.Disable();
        switch (actionMap)
        {
            case ActionMap.Player:
            {
                playerInputActions.Player.Enable();
                LockCursor();
                break;
            }
            case ActionMap.OpeningSequence:
            {
                playerInputActions.OpeningSequence.Enable();
                LockCursor();
                break;
            }
            case ActionMap.PlayerNightTime:
            {
                playerInputActions.PlayerNightTime.Enable();
                LockCursor();
                break;
            }
            case ActionMap.UI:
            {
                playerInputActions.UI.Enable();
                UnlockCursor();
                OnOpenUI?.Invoke();
                break;
            }
            case ActionMap.Printer:
            {
                playerInputActions.Printer.Enable();
                UnlockCursor();
                break;
            }
        }
    }

    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnEnable()
    {
        playerInputActions.Player.Interact.performed += PlayerInputActions_OnInteract;
        playerInputActions.Player.Pickup.performed += PlayerInputActions_OnPickupStarted;
        playerInputActions.Player.Pickup.canceled += PlayerInputActions_OnPickupStopped;
        playerInputActions.Player.Jump.performed += PlayerInputActions_OnJump;
        playerInputActions.Player.Click.performed += PlayerInputActions_OnClick;
        playerInputActions.Player.Pause.performed += PlayerInputActions_OnPause;

        playerInputActions.PlayerNightTime.Throw.performed += PlayerInputActions_OnThrow;
        playerInputActions.PlayerNightTime.Jump.performed += PlayerInputActions_OnJump;
        playerInputActions.PlayerNightTime.Sprint.started += PlayerInputActions_OnSprintStart;
        playerInputActions.PlayerNightTime.Sprint.canceled += PlayerInputActions_OnSprintStop;
        playerInputActions.PlayerNightTime.Attack.performed += PlayerInputActions_OnAttack;
        playerInputActions.PlayerNightTime.Dash.performed += PlayerInputActions_OnDash;

        playerInputActions.UI.Close.performed += PlayerInputActions_OnCloseUI;

        playerInputActions.Printer.Left.performed += PlayerInputActions_PrinterLeft;
        playerInputActions.Printer.Right.performed += PlayerInputActions_PrinterRight;
        playerInputActions.Printer.Top.performed += PlayerInputActions_PrinterTop;
        playerInputActions.Printer.Bottom.performed += PlayerInputActions_PrinterBottom;

        playerInputActions.OpeningSequence.Awake.performed += PlayerInputActions_OnAwake;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Interact.performed -= PlayerInputActions_OnInteract;
        playerInputActions.Player.Pickup.performed -= PlayerInputActions_OnPickupStarted;
        playerInputActions.Player.Pickup.canceled -= PlayerInputActions_OnPickupStopped;
        playerInputActions.Player.Jump.performed -= PlayerInputActions_OnJump;
        playerInputActions.Player.Click.performed -= PlayerInputActions_OnClick;
        playerInputActions.Player.Pause.performed -= PlayerInputActions_OnPause;

        playerInputActions.PlayerNightTime.Throw.performed -= PlayerInputActions_OnThrow;
        playerInputActions.PlayerNightTime.Jump.performed -= PlayerInputActions_OnJump;
        playerInputActions.PlayerNightTime.Sprint.started -= PlayerInputActions_OnSprintStart;
        playerInputActions.PlayerNightTime.Sprint.canceled -= PlayerInputActions_OnSprintStop;
        playerInputActions.PlayerNightTime.Attack.performed -= PlayerInputActions_OnAttack;
        playerInputActions.PlayerNightTime.Dash.performed -= PlayerInputActions_OnDash;

        playerInputActions.UI.Close.performed -= PlayerInputActions_OnCloseUI;

        playerInputActions.Printer.Left.performed -= PlayerInputActions_PrinterLeft;
        playerInputActions.Printer.Right.performed -= PlayerInputActions_PrinterRight;
        playerInputActions.Printer.Top.performed -= PlayerInputActions_PrinterTop;
        playerInputActions.Printer.Bottom.performed -= PlayerInputActions_PrinterBottom;

        playerInputActions.OpeningSequence.Awake.performed -= PlayerInputActions_OnAwake;
    }

    public Vector2 GetMovementVectorNormalized()
    {

        Vector2 movementVector = Vector2.zero; 
        
        if (playerInputActions.Player.enabled)
        {
            movementVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        }
        else if (playerInputActions.PlayerNightTime.enabled)
        {
            movementVector = playerInputActions.PlayerNightTime.Move.ReadValue<Vector2>();
        }

        movementVector = movementVector.normalized;
        return movementVector;
    }

    public Vector2 GetMouseMovement()
    {
        Vector2 mouseVector = Vector2.zero; 
        
        if (playerInputActions.Player.enabled)
        {
            mouseVector = playerInputActions.Player.Look.ReadValue<Vector2>();
        }
        else if (playerInputActions.PlayerNightTime.enabled)
        {
            mouseVector = playerInputActions.PlayerNightTime.Look.ReadValue<Vector2>();
        }

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

    private void PlayerInputActions_OnThrow(InputAction.CallbackContext _)
    {
        OnThrow?.Invoke();
    }

    private void PlayerInputActions_OnJump(InputAction.CallbackContext _)
    {
        OnJump?.Invoke();
    }

    private void PlayerInputActions_OnSprintStart(InputAction.CallbackContext _)
    {
        OnSprintStart?.Invoke();
    }

    private void PlayerInputActions_OnSprintStop(InputAction.CallbackContext _)
    {
        OnSprintStop?.Invoke();
    }

    private void PlayerInputActions_OnAttack(InputAction.CallbackContext _)
    {
        OnAttack?.Invoke();
    }

    private void PlayerInputActions_OnDash(InputAction.CallbackContext _)
    {
        OnDash?.Invoke();
    }

    private void PlayerInputActions_OnCloseUI(InputAction.CallbackContext _)
    {
        OnCloseUI?.Invoke();
    }

    public void PlayerInputActions_PrinterBottom(InputAction.CallbackContext _)
    {
        OnPrinterBottom?.Invoke();
    }

    public void PlayerInputActions_PrinterTop(InputAction.CallbackContext _)
    {
        OnPrinterTop?.Invoke();
    }

    public void PlayerInputActions_PrinterRight(InputAction.CallbackContext _)
    {
        OnPrinterRight?.Invoke();
    }

    public void PlayerInputActions_PrinterLeft(InputAction.CallbackContext _)
    {
        OnPrinterLeft?.Invoke();
    }

    public void PlayerInputActions_OnClick(InputAction.CallbackContext _)
    {
        OnClick?.Invoke();
    }

    public void PlayerInputActions_OnAwake(InputAction.CallbackContext _)
    {
        OnAwake?.Invoke();
    }

    public void PlayerInputActions_OnPause(InputAction.CallbackContext _)
    {
        OnPause?.Invoke();
    }
}
