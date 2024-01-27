using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider collider)
    {
        GameObject hitGameObject = collider.gameObject;
        if (hitGameObject.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
