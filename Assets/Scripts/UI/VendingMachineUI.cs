using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class VendingMachineUI : MonoBehaviour {
    [SerializeField] private PlayerUpgrades playerUpgrades;
    [SerializeField] private TMP_Text moneyText;

    [SerializeField] private TMP_Text remainingHealth;
    [SerializeField] private TMP_Text remainingMovementSpeed;
    [SerializeField] private TMP_Text remainingAttackSpeed;

    [SerializeField] private TMP_Text healthCost;
    [SerializeField] private TMP_Text movementSpeedCost;
    [SerializeField] private TMP_Text attackSpeedCost;


    private float showDelay = 0.8f;

    private void Start()
    {
        healthCost.text = "$" + playerUpgrades.healthCost;
        movementSpeedCost.text = "$" + playerUpgrades.movementSpeedCost;
        attackSpeedCost.text = "$" + playerUpgrades.attackSpeedCost;
    }

    private void OnEnable()
    {
        PlayerUpgrades.onBuy += OnBuy;
    }

    private void OnDisable()
    {
        PlayerUpgrades.onBuy -= OnBuy;
    }

    public void Show()
    {
        Invoke(nameof(_Show), showDelay);
    }

    private void _Show()
    {
        moneyText.text = "Money: $" + SaveData.Instance.data.playerBalance;
        remainingAttackSpeed.text = playerUpgrades.RemainingUpgradeAmount("AttackSpeed") + " Remaining"; 
        remainingMovementSpeed.text = playerUpgrades.RemainingUpgradeAmount("MovementSpeed") + " Remaining"; 
        remainingHealth.text = playerUpgrades.RemainingUpgradeAmount("Health") + " Remaining"; 
        gameObject.SetActive(true);
    }

    private void OnBuy(float amount, string name, int remaining)
    {
        SaveData.Instance.data.playerBalance -= amount;
        moneyText.text = "Money: $" + SaveData.Instance.data.playerBalance;

        switch (name)
        {
            case "AttackSpeed":
                remainingAttackSpeed.text = remaining + " Remaining"; 
                break;
            case "MovementSpeed":
                remainingMovementSpeed.text = remaining + " Remaining"; 
                break;
            case "Health":
                remainingHealth.text = remaining + " Remaining"; 
                break;
        }
    }
}
