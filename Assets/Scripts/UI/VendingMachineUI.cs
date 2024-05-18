using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class VendingMachineUI : MonoBehaviour {

    private float showDelay = 0.8f;

    public void Show()
    {
        Invoke(nameof(_Show), showDelay);
    }

    private void _Show()
    {
        gameObject.SetActive(true);
    }
}
