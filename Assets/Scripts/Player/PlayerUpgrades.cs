using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour, IInteractable
{
    public static event Action<float, string, int> onBuy;

    [SerializeField] private VendingMachineUI vendingMachineUI;
    [SerializeField] private GameObject vendingMachineCamera;

    private GameObject player;
    private Health playerHealth;
    private PlayerMovement playerMovement;

    public float healthCost = 1.50f;
    public float movementSpeedCost = 2.00f;
    public float attackSpeedCost = 2.50f;


    private int maxHealthUpgrades = 8;
    private int maxMovementSpeedUpgrades = 5;
    private int maxAttackSpeedUpgrades = 4;


    private int healthUpgradesBought = 0;
    private int movementSpeedUpgradesBought = 0;
    private int attackSpeedUpgradesBought = 0;

    private float maxHealthIncreaseAmount = 2;
    private float movementSpeedIncreaseAmount = 0.5f;
    private float attackSpeedIncreaseAmount = 0.05f;
    private float decreaseAttackDelayAmount = 0.05f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<Health>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        GameInput.Instance.OnCloseUI += CloseUI;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnCloseUI -= CloseUI;
    }

    private void CloseUI()
    {
        if (!vendingMachineUI.gameObject.activeSelf) return;
        vendingMachineCamera.SetActive(false);
        vendingMachineUI.gameObject.SetActive(false);
        GameInput.Instance.SwitchActionMaps(ActionMap.Player);
    }

    public void Interact(Vector3 playerPosition)
    {
        vendingMachineUI.Show();
        vendingMachineCamera.SetActive(true);
        GameInput.Instance.SwitchActionMaps(ActionMap.UI);
    }

    public void UpgradeHealth()
    {
        if (SaveData.Instance.data.playerBalance > healthCost && healthUpgradesBought < maxHealthUpgrades)
        {
            Debug.Log("Increase Health");
            SaveData.Instance.data.extraMaxHealth += maxHealthIncreaseAmount;
            healthUpgradesBought += 1;
            onBuy?.Invoke(healthCost, "Health", maxHealthUpgrades - healthUpgradesBought);
            SaveData.Instance.data.remainingHealthUpgrades = maxHealthUpgrades - healthUpgradesBought;
        }
    }

    public void UpgradeMovementSpeed()
    {
        if (SaveData.Instance.data.playerBalance > movementSpeedCost && movementSpeedUpgradesBought < maxMovementSpeedUpgrades)
        {
            Debug.Log("Increase Movement Speed");
            playerMovement.IncreaseMaxSpeed(movementSpeedIncreaseAmount);
            SaveData.Instance.data.extraMovementSpeed += movementSpeedIncreaseAmount;
            movementSpeedUpgradesBought += 1;
            onBuy?.Invoke(movementSpeedCost, "MovementSpeed", maxMovementSpeedUpgrades - movementSpeedUpgradesBought);
            SaveData.Instance.data.remainingMovementSpeedUpgrades = maxMovementSpeedUpgrades - movementSpeedUpgradesBought;
        }
    }

    public void UpgradeAttackSpeed()
    {
        if (SaveData.Instance.data.playerBalance > attackSpeedCost && attackSpeedUpgradesBought < maxAttackSpeedUpgrades)
        {
            Debug.Log("Increase Attack Speed");
            SaveData.Instance.data.extraAttackSpeed += attackSpeedIncreaseAmount;
            SaveData.Instance.data.decreasedAttackDelayAmount += decreaseAttackDelayAmount;
            attackSpeedUpgradesBought += 1;
            onBuy?.Invoke(attackSpeedCost, "AttackSpeed", maxAttackSpeedUpgrades - attackSpeedUpgradesBought);
            SaveData.Instance.data.remainingAttackSpeedUpgrades = maxAttackSpeedUpgrades - attackSpeedUpgradesBought;
        }
    }

    public int RemainingUpgradeAmount(string type)
    {
        switch (type)
        {
            case "Health":
                return maxHealthUpgrades - healthUpgradesBought;
            case "MovementSpeed":
                return maxMovementSpeedUpgrades - movementSpeedUpgradesBought;
            case "AttackSpeed":
                return maxAttackSpeedUpgrades - attackSpeedUpgradesBought;
        }
        Debug.LogError("Upgrade Type Doesn't Exist");
        return -1;
    }
}
