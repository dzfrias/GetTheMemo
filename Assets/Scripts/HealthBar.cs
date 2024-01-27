using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image healthBar;

    private float maxHealth;

    private void Start()
    {
        maxHealth = health.GetMaxHealth();
    }

    private void OnEnable()
    {
        health.OnHealthChanged += Health_OnHealthChanged;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= Health_OnHealthChanged;
    }

    private void Health_OnHealthChanged(float health)
    {
        healthBar.fillAmount = health/maxHealth;
    }
}
