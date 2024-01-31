using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprintBarUI : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    private Image sprintBarImage;
    private float maxSprintAmount;

    private void Awake()
    {
        sprintBarImage = GetComponent<Image>();
    }

    private void Start()
    {
        maxSprintAmount = playerMovement.GetMaxSprintTime();
        playerMovement.OnSprintTimeChanged += PlayerMovement_OnSprintTimeChanged;
    }

    private void PlayerMovement_OnSprintTimeChanged(float amount)
    {
        sprintBarImage.fillAmount = amount/maxSprintAmount;
    }
}
