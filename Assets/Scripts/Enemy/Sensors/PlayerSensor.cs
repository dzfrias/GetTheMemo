using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerSensor : MonoBehaviour
{
    public delegate void PlayerEnterEvent(Transform player);
    public delegate void PlayerExitEvent(Vector3 lastKnownPosition);
    public event PlayerEnterEvent onPlayerEnter;
    public event PlayerExitEvent onPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onPlayerEnter?.Invoke(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onPlayerExit?.Invoke(other.transform.position);
        }
    }
}
