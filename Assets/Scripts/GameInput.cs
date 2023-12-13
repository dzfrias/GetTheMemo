using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;

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

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 movementVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        movementVector = movementVector.normalized;
        return movementVector;
    }
}
