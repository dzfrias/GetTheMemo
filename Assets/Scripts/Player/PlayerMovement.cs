using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerMovement : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private bool jumpEnabled = false;
    [SerializeField] private float jumpPower = 1f;

    [Header("Sprint Settings")]
    [SerializeField] private float sprintMultiplier = 1.5f;

    [Header("Dash Settings")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private MMF_Player dashEffects;
    [SerializeField] private GameObject damageBox;
    private bool isDashing = false;

    private Vector3 playerVelocity;
    private PlayerCharges charges;
    private bool jump = false;
    private bool sprintInputPressed = false;

    private Transform camTransform;
    private CharacterController characterController;
    private PlayerMeleeAttack melee;

    private void Awake()
    {
        camTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
        charges = GetComponent<PlayerCharges>();
        melee = GetComponent<PlayerMeleeAttack>();
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
        if (melee != null && melee.IsAttacking())
        {
            playerVelocity *= 0.2f;
        }
    }

    private void HandleSprinting()
    {
        if (!sprintInputPressed) return;
        if (playerVelocity.x != 0 || playerVelocity.z != 0)
        {
            Vector3 sprintAppliedPlayerVelocity = new Vector3(playerVelocity.x * sprintMultiplier, playerVelocity.y, playerVelocity.z * sprintMultiplier);
            playerVelocity = sprintAppliedPlayerVelocity;
        }
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
        if (isDashing || GameInput.Instance.GetMovementVectorNormalized() == Vector2.zero) return;

        StartCoroutine(ActivateDashDuration());
    }

    private IEnumerator ActivateDashDuration()
    {
        if (!charges.UseCharge())
        {
            yield break;
        }
        if (melee != null)
        {
            melee.Cancel();
        }
        dashEffects.PlayFeedbacks();
        isDashing = true;
        LayerMask oldMask = characterController.excludeLayers;
        characterController.excludeLayers = oldMask | LayerMask.GetMask("Enemy");
        damageBox.SetActive(true);
        yield return new WaitForSeconds(dashDuration);
        characterController.excludeLayers = oldMask;
        isDashing = false;
        damageBox.SetActive(false);
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
