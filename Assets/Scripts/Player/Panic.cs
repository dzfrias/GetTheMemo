using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Panic : MonoBehaviour
{
    public event Action OnPanicStart;
    public event Action OnPanicEnd;

    [SerializeField] [Range(0f, 1f)] private float panicThreshold = 0.1f;

    private bool inPanic;
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnHealthChanged += TryPanic;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= TryPanic;
    }

    private void TryPanic(float currentHealth)
    {
        if (currentHealth / health.GetMaxHealth() <= panicThreshold && !inPanic)
        {
            inPanic = true;
            OnPanicStart?.Invoke();
        }
        if (currentHealth / health.GetMaxHealth() > panicThreshold && inPanic)
        {
            inPanic = false;
            OnPanicEnd?.Invoke();
        }
    }
}
