using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerCombo : MonoBehaviour
{
    public event Action<float> OnComboChanged;

    [SerializeField] private float maxCombo = 10f;
    [SerializeField] private float gainOnKill = 1.5f;
    [SerializeField] private float comboDecreaseSpeed = 0.2f;
    [SerializeField] private float comboDecreaseDelay = 3f;

    [Header("Effects")]
    [SerializeField] [Range(0f, 1f)] private float effectsThreshold;
    [SerializeField] private MMF_Player comboEffects;

    private float currentCombo;
    private Coroutine comboDecrease;
    private bool effectsOn;
    private Health health;

    public float GetMaxCombo()
    {
        return maxCombo;
    }

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDeath += AddToCombo;
        health.OnHealthChanged += ResetCombo;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= AddToCombo;
        health.OnHealthChanged -= ResetCombo;
    }

    private void ResetCombo(float health)
    {
        currentCombo = 0f;
        OnComboChanged?.Invoke(currentCombo);
        if (effectsOn)
        {
            comboEffects.SetDirectionBottomToTop();
            comboEffects.PlayFeedbacks();
        }
    }

    private void AddToCombo()
    {
        if (comboDecrease != null)
        {
            StopCoroutine(comboDecrease);
        }
        currentCombo = Mathf.Min(maxCombo, currentCombo + gainOnKill);
        if (currentCombo / maxCombo >= effectsThreshold && !effectsOn)
        {
            comboEffects.SetDirectionTopToBottom();
            comboEffects.PlayFeedbacks();
            effectsOn = true;
        }
        OnComboChanged?.Invoke(currentCombo);
        comboDecrease = StartCoroutine(ComboDecrease());
    }

    private IEnumerator ComboDecrease()
    {
        yield return new WaitForSeconds(comboDecreaseDelay);
        while (true)
        {
            currentCombo = Mathf.Max(0, currentCombo - comboDecreaseSpeed * Time.deltaTime);
            OnComboChanged?.Invoke(currentCombo);
            if (currentCombo / maxCombo < effectsThreshold && effectsOn)
            {
                comboEffects.SetDirectionBottomToTop();
                comboEffects.PlayFeedbacks();
                effectsOn = false;
            }
            yield return null;
        }
    }
}
