using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageBox : MonoBehaviour
{
    public event Action OnKill;
    public event Action OnHit;

    [SerializeField] private float damage = 1.5f;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<Health>(out Health health))
        {
            OnHit?.Invoke();
            health.TakeDamage(damage);
            if (health.IsDead())
            {
                OnKill?.Invoke();
            }
        }
    }
}
