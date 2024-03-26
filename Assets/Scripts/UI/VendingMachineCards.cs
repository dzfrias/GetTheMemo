using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineCards : MonoBehaviour
{
    public void Display(List<IAbility> abilities)
    {
        gameObject.SetActive(true);
        int i = 0;
        foreach (var ability in abilities)
        {
            Transform child = transform.GetChild(i);
            var card = child.gameObject.GetComponent<VendingMachineCard>();
            card.SetAbility(ability);
            i++;
        }
    }
}
