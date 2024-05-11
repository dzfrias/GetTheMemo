using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Bob : MonoBehaviour
{
    [SerializeField] private MMF_Player damageEffect;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnHealthChanged += TakeDamage;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= TakeDamage;
    }

    private void TakeDamage(float health)
    {
        damageEffect.PlayFeedbacks();
    }
}
