using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IGrabbable
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Pickup()
    {
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void Drop()
    {
        rb.useGravity = true;
        rb.velocity *= 2f;
        rb.interpolation = RigidbodyInterpolation.None;
    }
}
