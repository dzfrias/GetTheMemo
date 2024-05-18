using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSound : MonoBehaviour
{
    [SerializeField] private GameObject sound;

    private void OnCollisionEnter(Collision collision)
    {
        // Tiny collisions shouldn't count
        if (collision.relativeVelocity.magnitude < 1.5) return;
        ContactPoint contact = collision.contacts[0];
        Instantiate(sound, contact.point, Quaternion.identity);
    }
}
