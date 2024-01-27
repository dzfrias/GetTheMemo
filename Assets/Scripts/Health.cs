using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<float> OnHealthChanged;

    [SerializeField] private float maxHealth;

    private float health;

    private void Awake()
    {
        health = maxHealth;
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
            Debug.Log("DEAD");
        }
        OnHealthChanged?.Invoke(health);
    }
}
