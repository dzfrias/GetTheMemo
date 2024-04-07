using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float damage = 5f;

    private void Update()
    {
        transform.position += speed * transform.forward * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject hitGameObject = collider.gameObject;
        if (hitGameObject.TryGetComponent(out Health health) && hitGameObject.CompareTag("Player"))
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
