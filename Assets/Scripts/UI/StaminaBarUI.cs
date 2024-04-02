using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    private Image staminaBarImage;
    private float maxStaminaAmount;

    private void Awake()
    {
        staminaBarImage = GetComponent<Image>();
    }

    private void Start()
    {
        maxStaminaAmount = playerMovement.GetMaxStamina();
        playerMovement.OnStaminaChanged += PlayerMovement_OnStaminaChanged;
    }

    private void PlayerMovement_OnStaminaChanged(float amount)
    {
        staminaBarImage.fillAmount = amount/maxStaminaAmount;
    }
}
