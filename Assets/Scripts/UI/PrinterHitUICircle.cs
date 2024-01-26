using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterHitUICircle : MonoBehaviour
{
    [SerializeField] private GameObject toDisable;

    public void Disable()
    {
        toDisable.SetActive(false);
    }
}
