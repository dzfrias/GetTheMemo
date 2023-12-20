using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable, IHoverable, IGrabbable
{
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
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

    public void Pickup(Transform holdTransform)
    {
        Debug.Log("Picked up");
        transform.parent = holdTransform;
    }

    public void Drop()
    {
        Debug.Log("Dropped");
        transform.parent = null;
    }
}
