using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineUI : MonoBehaviour, IStationUI<VendingMachineData>
{
    [SerializeField] private VendingMachineRows rows;
    [SerializeField] private VendingMachineCards cards;

    private VendingMachineData data;

    public void Startup(VendingMachineData data)
    {
        this.data = data;
    }

    public ActionMap PreferredActionMap()
    {
        return ActionMap.UI;
    }

    public void OpenCards(int tier)
    {
        int tierMax = Enum.GetNames(typeof(AbilityTier)).Length - 1;
        if (tier > tierMax)
        {
            Debug.LogError("invalid card tier!");
            return;
        }

        rows.Hide();
        List<IAbility> abilities = data.GetTier((AbilityTier)tier);
        cards.Display(abilities);
    }

    private void OnDisable()
    {
        data.Shutdown();
    }
}
