using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    private Health health;
    private MMProgressBar progressBar;
    private float maxHealth;

    private void Awake()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        maxHealth = health.GetMaxShield();
        progressBar = GetComponent<MMProgressBar>();
    }

    private void OnEnable()
    {
        health.OnShieldChanged += Health_OnShieldChanged;
    }

    private void OnDisable()
    {
        health.OnShieldChanged -= Health_OnShieldChanged;
    }

    private void Health_OnShieldChanged(float health)
    {
        progressBar.UpdateBar(health, 0, maxHealth);
    }
}
