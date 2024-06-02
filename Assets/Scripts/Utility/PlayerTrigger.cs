using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private bool activateOnce;
    [SerializeField] private UnityEvent triggered;
    [SerializeField] private UnityEvent stopTriggered;

    private bool activated;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activated)
        {
            triggered.Invoke();
            if (activateOnce) activated = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            stopTriggered.Invoke();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && !activated)
        {
            triggered.Invoke();
            if (activateOnce) activated = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            stopTriggered.Invoke();
        }
    }
}
