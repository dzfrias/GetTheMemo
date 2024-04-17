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
    [SerializeField] private Transform attackPoint;
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

    private AttackState attackState = AttackState.None;
    private float attackDelay;

    public enum AttackState
    {
        None,
        NormalAttack,
        SuperAttackWindup,
        SuperAttack
    }

    private void Awake()
    {
        animator.TryGetComponent(out animationEventProxy);
        health = GetComponent<Health>();
        playerMovement = GetComponent<PlayerMovement>();
        attackDelay = playerCombatSO.normalAttackDelay;
        swordAnimationPool = playerCombatSO.normalSwordAttackAnimations;
    }

    private void OnEnable()
    {
        GameInput.Instance.OnAttack += Attack;
        GameInput.Instance.OnSuperAttackStart += SuperAttackWindup;
        GameInput.Instance.OnSuperAttackStop += SuperAttack;
        animationEventProxy.OnNormalAttack += DealNormalAttackDamage;
        animationEventProxy.OnSuperAttack += DealSuperAttackDamage;
        health.OnHealthChanged += Health_OnHealthChanged;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnAttack -= Attack;
        GameInput.Instance.OnSuperAttackStart -= SuperAttackWindup;
        GameInput.Instance.OnSuperAttackStop -= SuperAttack;
        animationEventProxy.OnNormalAttack -= DealNormalAttackDamage;
        animationEventProxy.OnSuperAttack -= DealSuperAttackDamage;
        health.OnHealthChanged -= Health_OnHealthChanged;
    }

    private void Update()
    {
        if (attackState.Equals(AttackState.NormalAttack) || attackState.Equals(AttackState.SuperAttack))
        {
            attackDelay -= Time.deltaTime;
            if (attackDelay <= 0)
            {
                SwitchAttackState(AttackState.None);
            }
        }
    }

    private void SwitchAttackState(AttackState attackState)
    {
        this.attackState = attackState;
        switch (attackState)
        {
            case AttackState.NormalAttack:
                attackDelay = playerCombatSO.normalAttackDelay;
                break;
            case AttackState.SuperAttack:
                attackDelay = playerCombatSO.superAttackDelay;
                break;
        }
    }

    private void Attack()
    {
        if (IsAttacking()) return;

        attackEffect.PlayFeedbacks();
        SwitchAttackState(AttackState.NormalAttack);

        PlayRandomAttackAnimation();
    }

    private void SuperAttackWindup()
    {
        if (IsAttacking()) return;

        SwitchAttackState(AttackState.SuperAttackWindup);
        animator.Play(playerCombatSO.superAttackWindupAnimation, 0, 0);
    }

    private void SuperAttack()
    {
        if (attackState != AttackState.SuperAttackWindup) return;

        attackEffect.PlayFeedbacks();
        SwitchAttackState(AttackState.SuperAttack);
        animator.Play(playerCombatSO.superAttackAnimation, 0, 0);
        playerMovement.UseStamina(playerCombatSO.superAttackStaminaCost);
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

    private void DealNormalAttackDamage()
    {
        DealDamage(playerCombatSO.normalAttackDamage, playerCombatSO.attackBox);
    }

    private void DealSuperAttackDamage()
    {
        DealDamage(playerCombatSO.superAttackDamage, playerCombatSO.superAttackBox);
    }

    private void DealDamage(float damage, Vector3 attackBox)
    {
        foreach (Collider collider in Physics.OverlapBox(attackPoint.position, attackBox/2, attackPoint.rotation, ~ignoreRaycast))
        {
            if (collider.TryGetComponent(out Health enemyHealth))
            {
                enemyHealth.TakeDamage(damage);
                effect.PlayFeedbacks();
                if (enemyHealth.GetHealth() <= 0)
                {
                    OnKillEnemy();
                }
            }
        }
    }

    public bool IsAttacking()
    {
        return attackState != AttackState.None;
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
