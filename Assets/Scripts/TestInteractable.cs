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
}
