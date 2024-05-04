using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class ComboBar : MonoBehaviour
{
    private PlayerCombo playerCombo;
    private MMProgressBar progressBar;

    private void Awake()
    {
        playerCombo = GameObject.FindWithTag("Player").GetComponent<PlayerCombo>();
        progressBar = GetComponent<MMProgressBar>();
    }

    private void OnEnable()
    {
        playerCombo.OnComboChanged += UpdateBar;
    }

    private void OnDisable()
    {
        playerCombo.OnComboChanged -= UpdateBar;
    }

    private void UpdateBar(float currentCombo)
    {
        progressBar.UpdateBar(currentCombo, 0, playerCombo.GetMaxCombo());
    }
}
