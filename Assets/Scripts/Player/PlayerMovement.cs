using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action<float> OnStaminaChanged;

    [Header("General Settings")]
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private bool jumpEnabled = false;
    [SerializeField] private float jumpPower = 1f;

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRegenerationSpeed = 3f;
    [SerializeField] private float staminaRegenerationCooldownTimeMax = 1f;

    [Header("Sprint Settings")]
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float sprintStaminaCost = 3f;

    [Header("Dash Settings")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashStaminaCost = 20f;
    private bool isDashing = false;

    private Vector3 playerVelocity;
    private bool jump = false;
    private bool sprintInputPressed = false;
    private float stamina;
    private float staminaRegenerationCooldownTime;

    private Transform camTransform;
    private CharacterController characterController;

    private void Awake()
    {
        camTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();

        stamina = maxStamina;
    }

    private void OnEnable()
    {
        GameInput.Instance.OnJump += GameInput_OnJump;
        GameInput.Instance.OnSprintStart += GameInput_OnSprintStart;
        GameInput.Instance.OnSprintStop += GameInput_OnSprintStop;
        GameInput.Instance.OnDash += GameInput_OnDash;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnJump -= GameInput_OnJump;
        GameInput.Instance.OnSprintStart -= GameInput_OnSprintStart;
        GameInput.Instance.OnSprintStop -= GameInput_OnSprintStop;
        GameInput.Instance.OnDash -= GameInput_OnDash;
    }

    private void Update()
    {
        RotateToCamera();

        if (isDashing)
        {
            ApplyDash();
            return;
        }

        if (jump)
        {
            playerVelocity.y += Mathf.Sqrt(jumpPower * -1.0f * Physics.gravity.y);
        }

        ApplyMovement();
        ApplyGravity();
        HandleSprinting();

        characterController.Move(playerVelocity.normalized * movementSpeed * Time.deltaTime);

        jump = false;

        HandleStaminaRegeneration();
    }

    private void ApplyDash()
    {
        characterController.Move(playerVelocity.normalized * dashForce * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        Vector2 movementVectorNormalized = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 movement = new Vector3(movementVectorNormalized.x, 0f, movementVectorNormalized.y);
        movement = camTransform.forward * movement.z + camTransform.right * movement.x;
        playerVelocity.x = movement.x * movementSpeed;
        playerVelocity.z = movement.z * movementSpeed;
    }

    private void HandleSprinting()
    {
        if (sprintInputPressed)
        {
            if (stamina > 0 && (playerVelocity.x != 0 || playerVelocity.z != 0))
            {
                Vector3 sprintAppliedPlayerVelocity = new Vector3(playerVelocity.x * sprintMultiplier, playerVelocity.y, playerVelocity.z * sprintMultiplier);
                playerVelocity = sprintAppliedPlayerVelocity;
                UseStamina(sprintStaminaCost * Time.deltaTime);
            }
        }
    }

    private void HandleStaminaRegeneration()
    {
        if (staminaRegenerationCooldownTime <= 0)
        {
            RegenerateStamina(staminaRegenerationSpeed * Time.deltaTime);
        }
        else
        {
            staminaRegenerationCooldownTime -= Time.deltaTime;
        }
    }

    private void UseStamina(float amount)
    {
        stamina -= amount;
        if (stamina <= 0)
        {
            stamina = 0;
        }
        staminaRegenerationCooldownTime = staminaRegenerationCooldownTimeMax;
        OnStaminaChanged?.Invoke(stamina);
    }

    private void RegenerateStamina(float amount)
    {
        stamina += amount;
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
        OnStaminaChanged?.Invoke(stamina);
    }

    public float GetMaxStamina()
    {
        return maxStamina;
    }

    private void GameInput_OnSprintStart()
    {
        sprintInputPressed = true;
    }

    private void GameInput_OnSprintStop()
    {
        sprintInputPressed = false;
    }

    private void GameInput_OnJump()
    {
        if (jumpEnabled && characterController.isGrounded)
        {
            jump = true;
        }
    }

    private void GameInput_OnDash()
    {
        if (isDashing || stamina <= 0) return;

        StartCoroutine(ActivateDashDuration());
    }

    private IEnumerator ActivateDashDuration()
    {
        isDashing = true;
        UseStamina(dashStaminaCost);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    private void RotateToCamera()
    {
        transform.rotation = Quaternion.Euler(0f, camTransform.rotation.eulerAngles.y, 0f);
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        }
        else if (playerVelocity.y != -1)
        {
            playerVelocity.y = -1f;
        }
    }
}
