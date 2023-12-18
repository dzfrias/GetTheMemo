using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable, IGrabbable
{
    private Outline outline;
    private BoxCollider boxCollider;
    private Rigidbody rb;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
    }

    public void Interact(PlayerInteraction playerInteraction)
    {
        playerInteraction.SetHeldObject(gameObject);

        DisableMovement();
    }

    public void ShowOutline()
    {
        outline.enabled = true;
    }

    public void HideOutline()
    {
        outline.enabled = false;
    }

    public void EnableMovement()
    {
        transform.parent = null;
        boxCollider.enabled = true;
        rb.isKinematic = false;
    }

    public void DisableMovement()
    {
        boxCollider.enabled = false;
        rb.isKinematic = true;
    }
}
