using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    [SerializeField] private Color hoverColor;

    private Color normalColor;
    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
        normalColor = img.color;
    }

    public void OnHover()
    {
        img.color = hoverColor;
    }

    public void Reset()
    {
        img.color = normalColor;
    }
}
