using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharges : MonoBehaviour
{
    public static event Action<int> OnChargesChanged;
    public static event Action<int> OnMaxChargesChanged;

    [SerializeField] private int maxCharges = 3;

    private int currentCharges;

    private void Start()
    {
        currentCharges = maxCharges;
        OnMaxChargesChanged?.Invoke(maxCharges);
        OnChargesChanged?.Invoke(currentCharges);
    }

    public void GainCharge()
    {
        if (currentCharges == maxCharges) return;
        currentCharges++;
        OnChargesChanged?.Invoke(currentCharges);
    }

    public int GetCharges()
    {
        return currentCharges;
    }

    public bool CanUseCharge()
    {
        return currentCharges != 0;
    }

    public bool UseCharge()
    {
        if (currentCharges == 0) return false;

        currentCharges--;
        OnChargesChanged?.Invoke(currentCharges);
        return true;
    }
}
