using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable, IHoverable, IGrabbable
{
    private Outline outline;
    private Rigidbody rb;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        rb = GetComponent<Rigidbody>();
    }

    public void Interact()
    {
        Debug.Log("Interacted");
        TaskSystemUI.Instance.CompleteTask(0);
    }

    public void Hover()
    {
        outline.enabled = true;
    }

    public void NoHover()
    {
        outline.enabled = false;
    }

    public void Pickup()
    {
        rb.useGravity = false;
    }

    public void Drop()
    {
        rb.useGravity = true;
    }
}
