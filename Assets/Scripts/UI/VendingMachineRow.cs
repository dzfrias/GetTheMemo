using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachineRow : MonoBehaviour
{
    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void OnDisable()
    {
        // Have to do this because of C# value type behavior...
        Color color = img.color;
        color.a = 0f;
        img.color = color;
    }
}
