using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;


    private MMProgressBar sprintProgressBar;
    private Image staminaBarImage;
    private float maxStaminaAmount;

    private void Awake()
    {
        staminaBarImage = GetComponent<Image>();
        sprintProgressBar = GetComponent<MMProgressBar>();
    }

    private void Start()
    {
        maxStaminaAmount = playerMovement.GetMaxStamina();
        playerMovement.OnStaminaChanged += PlayerMovement_OnStaminaChanged;
    }

    private void PlayerMovement_OnStaminaChanged(float amount)
    {
        sprintProgressBar.UpdateBar(amount, 0, maxStaminaAmount);
        staminaBarImage.fillAmount = amount/maxStaminaAmount;
    }
}
