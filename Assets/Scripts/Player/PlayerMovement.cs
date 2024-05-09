using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerMovement : MonoBehaviour
{
    private enum DashDirection
    {
        Forwards,
        NotForwards,
    }

    [Header("General Settings")]
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float gravityMultiplier = 2f;

    [Header("Jump Settings")]
    [SerializeField] private bool jumpEnabled = false;
    [SerializeField] private float jumpPower = 4f;
    [SerializeField] private MMF_Player jumpEffects;

    [Header("Sprint Settings")]
    [SerializeField] private float sprintMultiplier = 1.5f;

    [Header("Dash Settings")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private MMF_Player dashEffects;
    [SerializeField] private DamageBox damageBox;
    private DashDirection? dashDirection = null;

    private Vector3 playerVelocity;
    private PlayerCharges charges;
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

    private void Start()
    {
        movementSpeed += SaveData.Instance.data.extraMovementSpeed;
    }

    private void OnEnable()
    {
        GameInput.Instance.OnJump += GameInput_OnJump;
        GameInput.Instance.OnSprintStart += GameInput_OnSprintStart;
        GameInput.Instance.OnSprintStop += GameInput_OnSprintStop;
        GameInput.Instance.OnDash += GameInput_OnDash;
        if (damageBox != null)
        {
            damageBox.OnKill += () => charges.GainCharge();
        }
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

        if (IsDashing())
        {
            ApplyDash();
            return;
        }


        ApplyMovement();
        ApplyGravity();
        HandleSprinting();

        characterController.Move(playerVelocity * Time.deltaTime);
    }

    public bool IsWalking()
    {
        return characterController.isGrounded && GameInput.Instance.GetMovementVectorNormalized() != Vector2.zero;
    }

    private void ApplyDash()
    {
        Vector3 moveDirection = Vector3.zero;
        // If the player is dashing forwards, we use the camera's forward
        // direction (so the player can dash upwards, if needed)
        switch (dashDirection)
        {
            case DashDirection.Forwards:
                moveDirection = camTransform.forward;
                break;
            case DashDirection.NotForwards:
                moveDirection = playerVelocity.normalized;
                break;
        }
        characterController.Move(moveDirection * dashForce * Time.deltaTime);
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
            playerVelocity *= 0.8f;
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
            playerVelocity.y = Mathf.Sqrt(-jumpPower * Physics.gravity.y);
            jumpEffects.PlayFeedbacks();
            if (melee != null)
            {
                melee.Cancel();
            }
        }
    }

    private void GameInput_OnDash()
    {
        if (IsDashing() || GameInput.Instance.GetMovementVectorNormalized() == Vector2.zero) return;

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
        // Player going forwards
        if (GameInput.Instance.GetMovementVectorNormalized().y == 1)
        {
            dashDirection = DashDirection.Forwards;
        }
        else
        {
            dashDirection = DashDirection.NotForwards;
        }
        LayerMask oldMask = characterController.excludeLayers;
        characterController.excludeLayers = oldMask | LayerMask.GetMask("Enemy");
        damageBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(dashDuration);
        characterController.excludeLayers = oldMask;
        dashDirection = null;
        damageBox.gameObject.SetActive(false);
    }

    private bool IsDashing()
    {
        return dashDirection != null;
    }

    private void RotateToCamera()
    {
        transform.rotation = Quaternion.Euler(0f, camTransform.rotation.eulerAngles.y, 0f);
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded && !IsDashing())
        {
            playerVelocity.y += gravityMultiplier * Physics.gravity.y * Time.deltaTime;
        }
    }

    internal void IncreaseMaxSpeed(int amount)
    {
        movementSpeed += amount;
    }
}
