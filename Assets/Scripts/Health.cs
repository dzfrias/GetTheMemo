using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnDeath;
    public event Action<float> OnHealthChanged;

    [SerializeField] private float maxHealth;

    [Header("Shield")]
    [SerializeField] private float shieldAmount;
    [SerializeField] private float shieldRegen = 0.2f;
    [SerializeField] private float regenBufferTime = 1f;

    private float health;

    private void Awake()
    {
        health = maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            OnDeath?.Invoke();
            Debug.Log("DEAD");
        }
        OnHealthChanged?.Invoke(health);
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        OnHealthChanged?.Invoke(health);
    }
}
