using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningCamera : MonoBehaviour
{
    public bool canContinue;

    private void Start()
    {
        GameInput.Instance.SwitchActionMaps(ActionMap.OpeningSequence);
    }

    private void OnEnable()
    {
        GameInput.Instance.OnAwake += GameInput_OnAwake;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnAwake -= GameInput_OnAwake;
    }

    private void GameInput_OnAwake()
    {
        if (!canContinue) return;
        gameObject.SetActive(false);
        GameInput.Instance.SwitchActionMaps(ActionMap.Player);
        OfficeManager.Instance.NextBeat();
    }

    public void CanContinue()
    {
        canContinue = true;
    }
}
