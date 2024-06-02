using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Health health;
    private MMProgressBar progressBar;
    private float maxHealth;

    private void Awake()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        maxHealth = health.GetMaxHealth();
        progressBar = GetComponent<MMProgressBar>();
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
        progressBar.UpdateBar(health, 0, maxHealth);
    }
}
