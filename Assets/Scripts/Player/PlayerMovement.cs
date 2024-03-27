using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action<float> OnSprintTimeChanged;

    [Header("Movement Settings")]
    [SerializeField]
    private float movementSpeed = 1f;
    
    [SerializeField]
    private bool jumpEnabled = false;

    [SerializeField]
    private float jumpPower = 1f;

    [SerializeField]
    private float sprintMultiplier = 1.5f;

    [SerializeField]
    private float sprintTimeMax = 3f;

    [SerializeField]
    private float sprintRegenSpeed = 3f;

    [SerializeField]
    private float sprintCooldown = 2f;
    [Header("Dash Settings")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    private bool isDashing = false;

    private Vector3 playerVelocity;
    private bool jump = false;
    private bool sprintInputPressed = false;
    private float sprintTime;
    private bool onSprintCooldown = false;

    private Transform camTransform;
    private CharacterController characterController;

    private void Awake()
    {
        camTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();

        sprintTime = sprintTimeMax;
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
            if (sprintTime > 0 && (playerVelocity.x != 0 || playerVelocity.z != 0))
            {
                Vector3 sprintAppliedPlayerVelocity = new Vector3(playerVelocity.x * sprintMultiplier, playerVelocity.y, playerVelocity.z * sprintMultiplier);
                playerVelocity = sprintAppliedPlayerVelocity;
                sprintTime -= Time.deltaTime;
                OnSprintTimeChanged?.Invoke(sprintTime);
            }
            else if (!onSprintCooldown)
            {
                StartCoroutine(SprintCooldown());
            }
        }
        else if (sprintTime < sprintTimeMax && !onSprintCooldown)
        {
            sprintTime += Time.deltaTime * sprintRegenSpeed;
            OnSprintTimeChanged?.Invoke(sprintTime);
        }
    }

    private IEnumerator SprintCooldown()
    {
        onSprintCooldown = true;
        yield return new WaitForSeconds(sprintCooldown);
        onSprintCooldown = false;
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
        if (isDashing) return;

        StartCoroutine(ActivateDashDuration());
    }

    private IEnumerator ActivateDashDuration()
    {
        isDashing = true;
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

    public float GetMaxSprintTime()
    {
        return sprintTimeMax;
    }
}
