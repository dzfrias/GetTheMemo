using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    private Outline outline;
    private BoxCollider boxCollider;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Interact(PlayerInteraction playerInteraction)
    {
        playerInteraction.SetHeldObject(gameObject);
        Debug.Log("Interacted");

        boxCollider.enabled = false;
    }

    public void ShowOutline()
    {
        outline.enabled = true;
    }

    public void HideOutline()
    {
        outline.enabled = false;
    }
}
