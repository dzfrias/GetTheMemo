using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnDeath;
    public event Action<float> OnHealthChanged;
    public event Action<float> OnShieldChanged;

    [SerializeField] private float maxHealth;
    [SerializeField] private float invincibility;

    [Header("Shield")]
    [SerializeField] private float maxShield;
    [SerializeField] private float shieldRegen = 0.5f;
    [SerializeField] private float regenBufferTime = 1f;

    private float health;
    private float shieldAmount;
    private Coroutine regen;
    private bool invincible;

    private void Awake()
    {
        health = maxHealth;
        shieldAmount = maxShield;
    }

    private IEnumerator Regen()
    {
        yield return new WaitForSeconds(regenBufferTime);

        while (shieldAmount < maxShield)
        {
            shieldAmount += shieldRegen * Time.deltaTime;
            OnShieldChanged?.Invoke(shieldAmount);
            yield return null;
        }
        // Just to make sure we don't go over
        shieldAmount = maxShield;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetShield()
    {
        return shieldAmount;
    }

    public float GetMaxShield()
    {
        return maxShield;
    }

    public void TakeDamage(float amount)
    {
        if (invincible) return;
        if (regen != null)
        {
            StopCoroutine(regen);
        }
        regen = StartCoroutine(Regen());
        var toTake = shieldAmount - amount;
        shieldAmount = Mathf.Max(0f, toTake);
        OnShieldChanged?.Invoke(shieldAmount);
        // If the user still has shield left, take no damage
        if (toTake >= 0)
        {
            return;
        }
        StartCoroutine(Invincible());
        // This will be a negative number, so it will subtract what the shield
        // did not take
        health += toTake;
        if (health <= 0)
        {
            OnDeath?.Invoke();
            Debug.Log("DEAD");
        }
        OnHealthChanged?.Invoke(health);
    }

    public bool IsDead()
    {
        return health <= 0;
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

    private IEnumerator Invincible()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibility);
        invincible = false;
    }
}
