using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundShockHitbox : MonoBehaviour
{
    public event Action<Health> OnHit;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Health health))
        {
            OnHit?.Invoke(health);
        }
    }
}
