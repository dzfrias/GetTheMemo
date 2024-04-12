using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] private List<string> swordAnimations;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask ignoreRaycast;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform head;
    [SerializeField] private float attackDistance = 5f;
    [SerializeField] private float attackDelayMax = 0.75f;
    private float attackDelay;
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private MMF_Player effect;
    [SerializeField] private MMF_Player damageEffect;
    [SerializeField] private MMF_Player attackEffect;
    private bool isAttacking = false;
    private List<string> swordAnimationPool;

    private AnimationEventProxy animationEventProxy;
    private Health health;

    private void Awake()
    {
        animator.TryGetComponent(out animationEventProxy);
        health = GetComponent<Health>();
        attackDelay = attackDelayMax;
        swordAnimationPool = swordAnimations;
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
                attackDelay = attackDelayMax;
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
        if (Physics.Raycast(ray, out RaycastHit raycastHit, attackDistance, ~ignoreRaycast))
        {
            if (raycastHit.collider.TryGetComponent(out Health health))
            {
                health.TakeDamage(attackDamage);
                effect.PlayFeedbacks();
            }
            Debug.Log("Hit: " + raycastHit.collider.gameObject.name);
        }
    }

    private void Health_OnHealthChanged(float health)
    {
        damageEffect.PlayFeedbacks();
    }
}
