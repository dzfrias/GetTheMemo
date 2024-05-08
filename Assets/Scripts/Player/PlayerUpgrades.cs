using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject vendingMachineUI;
    [SerializeField] private GameObject vendingMachineCamera;

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
        vendingMachineUI.SetActive(false);
        vendingMachineCamera.SetActive(false);
        GameInput.Instance.SwitchActionMaps(ActionMap.Player);
    }

    public void Interact(Vector3 playerPosition)
    {
        vendingMachineUI.SetActive(true);
        vendingMachineCamera.SetActive(true);
        GameInput.Instance.SwitchActionMaps(ActionMap.UI);
    }

    public void UpgradeHealth()
    {
        Debug.Log("Increase Health");
    }

    public void UpgradeMovementSpeed()
    {
        Debug.Log("Increase Movement Speed");
    }
}
