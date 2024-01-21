using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineHover : MonoBehaviour, IHoverable
{
    private Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
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
