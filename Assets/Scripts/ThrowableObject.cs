using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    [SerializeField] private float damage;

    private Rigidbody rb;

    private bool isThrown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 direction, float power)
    {
        rb.AddForce(direction * power, ForceMode.Impulse);
        isThrown = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!isThrown) return;

        if (collider.TryGetComponent(out PrinterBossStunTracker printerBossStunTracker))
        {
            printerBossStunTracker.IncreaseStunAmount();
            Destroy(gameObject);
        }
    }
}
