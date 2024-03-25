using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineCard : MonoBehaviour
{
    private IAbility ability;

    public void SetAbility(IAbility ability)
    {
        this.ability = ability;
    }

    public void GetAbility()
    {
        AbilitiesHolder playerAbilities = GameObject.FindWithTag("Player").GetComponent<AbilitiesHolder>();
        playerAbilities.Add(ability);
        gameObject.SetActive(false);
    }
}
