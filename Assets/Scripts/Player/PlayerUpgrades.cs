using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour, IInteractable
{
    [SerializeField] private VendingMachineUI vendingMachineUI;
    [SerializeField] private GameObject vendingMachineCamera;

    private GameObject player;
    private Health playerHealth;
    private PlayerMovement playerMovement;

    private int maxHealthIncreaseAmount = 2;
    private int movementSpeedIncreaseAmount = 1;
    private float attackSpeedIncreaseAmount = 0.1f;
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
        if (!vendingMachineUI.activeSelf) return;
        vendingMachineUI.Hide();
        vendingMachineCamera.SetActive(false);
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
        Debug.Log("Increase Health");
        SaveData.Instance.data.extraMaxHealth += maxHealthIncreaseAmount;
    }

    public void UpgradeMovementSpeed()
    {
        Debug.Log("Increase Movement Speed");
        playerMovement.IncreaseMaxSpeed(movementSpeedIncreaseAmount);
        SaveData.Instance.data.extraMovementSpeed += movementSpeedIncreaseAmount;
    }

    public void UpgradeAttackSpeed()
    {
        Debug.Log("Increase Attack Speed");
        SaveData.Instance.data.extraAttackSpeed += attackSpeedIncreaseAmount;
        SaveData.Instance.data.decreasedAttackDelayAmount += decreaseAttackDelayAmount;
    }
}
