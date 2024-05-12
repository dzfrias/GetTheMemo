using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharges : MonoBehaviour
{
    public static event Action<int> OnChargesChanged;
    public static event Action<int> OnMaxChargesChanged;

    [SerializeField] private int maxCharges = 3;
    [SerializeField] private bool autoRefill;

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
        if (autoRefill && currentCharges == 0)
        {
            StartCoroutine(Refill());
        }
        return true;
    }

    private IEnumerator Refill()
    {
        yield return new WaitForSeconds(1f);
        currentCharges = maxCharges;
        OnChargesChanged?.Invoke(currentCharges);
    }
}
