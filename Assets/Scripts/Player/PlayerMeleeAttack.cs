using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerMeleeAttack : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private PlayerCombatSO playerCombatSO;
    
    [Header("Damage Layer Mask")]
    [SerializeField] private LayerMask ignoreRaycast;

    [Header("Other Objects")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform head;

    [Header("Other Components")]
    [SerializeField] private Animator animator;

    [Header("Effects")]
    [SerializeField] private MMF_Player effect;
    [SerializeField] private MMF_Player damageEffect;
    [SerializeField] private MMF_Player attackEffect;

    private List<string> swordAnimationPool;

    private AnimationEventProxy animationEventProxy;
    private Health health;
    private PlayerMovement playerMovement;

    private bool isAttacking = false;
    private float attackDelay;

    private void Awake()
    {
        animator.TryGetComponent(out animationEventProxy);
        health = GetComponent<Health>();
        playerMovement = GetComponent<PlayerMovement>();
        attackDelay = playerCombatSO.attackDelay;
        swordAnimationPool = playerCombatSO.swordAnimations;
    }

    private void OnEnable()
    {
        GameInput.Instance.OnAttack += Attack;
        animationEventProxy.OnAttack += HitObject;
        health.OnHealthChanged += Health_OnHealthChanged;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnAttack -= Attack;
        animationEventProxy.OnAttack -= HitObject;
        health.OnHealthChanged -= Health_OnHealthChanged;
    }

    private void Update()
    {
        if (isAttacking)
        {
            attackDelay -= Time.deltaTime;
            if (attackDelay <= 0)
            {
                isAttacking = false;
                attackDelay = playerCombatSO.attackDelay;
            }
        }
    }

    private void Attack()
    {
        if (isAttacking) return;

        attackEffect.PlayFeedbacks();
        isAttacking = true;

        PlayRandomAttackAnimation();
    }

    private void PlayRandomAttackAnimation()
    {
        int lastIndex = swordAnimationPool.Count - 1;
        int randomIndex = UnityEngine.Random.Range(0, lastIndex);
        string randomAnimation = swordAnimationPool[randomIndex];
        
        swordAnimationPool[randomIndex] = swordAnimationPool[lastIndex];
        swordAnimationPool[lastIndex] = randomAnimation;

        animator.Play(randomAnimation, 0, 0);
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    private void HitObject()
    {
        Vector3 middleOfScreen = new Vector3(0.5f, 0.5f, 0f);
        Ray ray = cam.ViewportPointToRay(middleOfScreen);
        Debug.DrawRay(ray.GetPoint(0), ray.direction, Color.red, 5f);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, playerCombatSO.attackDistance, ~ignoreRaycast))
        {
            if (raycastHit.collider.TryGetComponent(out Health enemyHealth))
            {
                enemyHealth.TakeDamage(playerCombatSO.damage);
                effect.PlayFeedbacks();
                if (enemyHealth.GetHealth() <= 0)
                {
                    OnKillEnemy();
                }
            }
            Debug.Log("Hit: " + raycastHit.collider.gameObject.name);
        }
    }

    private void OnKillEnemy()
    {
        health.Heal(playerCombatSO.healAmountOnKill);
        playerMovement.RegenerateStamina(playerCombatSO.staminaIncreaseAmountOnKill);
    }

    private void Health_OnHealthChanged(float health)
    {
        damageEffect.PlayFeedbacks();
    }
}
