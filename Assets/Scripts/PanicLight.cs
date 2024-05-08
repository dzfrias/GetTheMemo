using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class PanicLight : MonoBehaviour
{
    [SerializeField] private Color panicColor;

    private Color originalColor;
    private new Light light;
    private Panic playerPanic;

    private void Awake()
    {
        light = GetComponent<Light>();
        originalColor = light.color;
        playerPanic = GameObject.FindGameObjectWithTag("Player").GetComponent<Panic>();
    }

    private void OnEnable()
    {
        playerPanic.OnPanicStart += ChangeLight;
        playerPanic.OnPanicEnd += RestoreLight;
    }

    private void OnDisable()
    {
        playerPanic.OnPanicStart -= ChangeLight;
        playerPanic.OnPanicEnd -= RestoreLight;
    }

    private void ChangeLight()
    {
        light.color = panicColor;
    }

    private void RestoreLight()
    {
        light.color = originalColor;
    }
}
