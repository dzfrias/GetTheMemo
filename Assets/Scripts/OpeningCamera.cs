using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningCamera : MonoBehaviour
{
    private void Start()
    {
        GameInput.Instance.SwitchActionMaps(ActionMap.OpeningSequence);
        GameInput.Instance.OnAwake += GameInput_OnAwake;
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
        gameObject.SetActive(false);
        GameInput.Instance.SwitchActionMaps(ActionMap.Player);
        LevelManager.Instance.NextBeat();
    }
}
