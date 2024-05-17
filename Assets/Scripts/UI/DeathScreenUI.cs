using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenUI : MonoBehaviour
{
    [SerializeField] private Button retryBtn;

    private void OnEnable()
    {
        retryBtn.onClick.AddListener(OnRetry);
    }

    private void OnDisable()
    {
        retryBtn.onClick.RemoveListener(OnRetry);
    }

    private void OnRetry()
    {
        OfficeManager.Instance.RestartScene();
    }
}
