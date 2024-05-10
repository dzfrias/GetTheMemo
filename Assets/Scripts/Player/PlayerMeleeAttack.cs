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
    [SerializeField] private Transform head;

    [Header("Other Components")]
    [SerializeField] private Animator animator;

    [Header("Effects")]
    [SerializeField] private MMF_Player effect;
    [SerializeField] private MMF_Player damageEffect;
    [SerializeField] private MMF_Player attackEffect;
    [SerializeField] private MMF_Player superAttackEffect;

    private List<string> swordAnimationPool;

    private AnimationEventProxy animationEventProxy;
    private Health health;
    private PlayerCharges charges;
    private Camera cam;

    private AttackState attackState = AttackState.None;
    private float attackDelay;
    private float normalAttackDelay;
    private float windupTime;
    private float attackSpeed = 1;

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
        charges = GetComponent<PlayerCharges>();
        attackDelay = playerCombatSO.normalAttackDelay;
        swordAnimationPool = playerCombatSO.normalSwordAttackAnimations;
        cam = Camera.main;
    }

    private void Start()
    {
        attackSpeed += SaveData.Instance.data.extraAttackSpeed;
        animator.SetFloat("AttackSpeed", attackSpeed);

        normalAttackDelay = playerCombatSO.normalAttackDelay -= SaveData.Instance.data.decreasedAttackDelayAmount;
    }

    private void OnEnable()
    {
        GameInput.Instance.OnAttack += Attack;
        GameInput.Instance.OnSuperAttackStart += SuperAttackWindup;
        GameInput.Instance.OnSuperAttackStop += SuperAttack;
        animationEventProxy.OnPrimaryAttack += DealNormalAttackDamage;
        animationEventProxy.OnSecondaryAttack += DealSuperAttackDamage;
        health.OnHealthChanged += Health_OnHealthChanged;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnAttack -= Attack;
        GameInput.Instance.OnSuperAttackStart -= SuperAttackWindup;
        GameInput.Instance.OnSuperAttackStop -= SuperAttack;
        animationEventProxy.OnPrimaryAttack -= DealNormalAttackDamage;
        animationEventProxy.OnSecondaryAttack -= DealSuperAttackDamage;
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

        if (attackState.Equals(AttackState.SuperAttackWindup))
        {
            windupTime += Time.deltaTime;
        }
    }

    private void SwitchAttackState(AttackState attackState)
    {
        this.attackState = attackState;
        switch (attackState)
        {
            case AttackState.None:
                animator.Play(playerCombatSO.defaultSwordPositionAnimation, 0, 0);
                break;
            case AttackState.NormalAttack:
                attackDelay = normalAttackDelay;
                PlayRandomAttackAnimation();
                break;
            case AttackState.SuperAttack:
                attackDelay = playerCombatSO.superAttackDelay;
                animator.Play(playerCombatSO.superAttackAnimation, 0, 0);
                break;
            case AttackState.SuperAttackWindup:
                animator.Play(playerCombatSO.superAttackWindupAnimation, 0, 0);
                break;
        }
    }

    public void Cancel()
    {
        SwitchAttackState(AttackState.None);
    }

    private void Attack()
    {
        if (IsAttacking()) return;

        attackEffect.PlayFeedbacks();
        SwitchAttackState(AttackState.NormalAttack);
    }

    private void SuperAttackWindup()
    {
        if (IsAttacking() || !charges.CanUseCharge()) return;

        SwitchAttackState(AttackState.SuperAttackWindup);
    }

    private void SuperAttack()
    {
        if (attackState != AttackState.SuperAttackWindup) return;

        if (windupTime < playerCombatSO.minimumSuperAttackWindupTime || !charges.UseCharge())
        {
            windupTime = 0;
            SwitchAttackState(AttackState.None);
            return;
        }

        superAttackEffect.PlayFeedbacks();
        SwitchAttackState(AttackState.SuperAttack);
        windupTime = 0;
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
        charges.GainCharge();
    }

    private void Health_OnHealthChanged(float health)
    {
        damageEffect.PlayFeedbacks();
    }
}
