using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void Interact()
    {
        Debug.Log("Interacted");
    }

    public void ShowOutline()
    {
        outline.OutlineMode = Outline.Mode.OutlineVisible;
    }

    public void HideOutline()
    {
        outline.OutlineMode = Outline.Mode.OutlineHidden;
    }
}
