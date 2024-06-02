using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour, IGrabbable
{
    [SerializeField] private float throwMultiplier = 2f;

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
        rb.velocity *= throwMultiplier;
        rb.interpolation = RigidbodyInterpolation.None;
    }
}
