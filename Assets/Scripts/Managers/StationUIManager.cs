using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationUIManager : MonoBehaviour
{
    public static StationUIManager Instance;

    private GameObject currentStationUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There is more than 1 StationUIManager in the scene");
        }
    }

    public void Startup<T>(T data)
    {
        if (currentStationUI != null)
        {
            Debug.LogError("There is already a station UI open!");
            return;
        }

        foreach (Transform child in transform)
        {
            if (child.GetComponent<IStationUI<T>>() != null)
            {
                child.gameObject.SetActive(true);
                child.GetComponent<IStationUI<T>>().Startup(data);
                currentStationUI = child.gameObject;
                GameInput.Instance.SwitchActionMaps();
            }
        }
    }

    public void CloseCurrentStationUI()
    {
        currentStationUI.SetActive(false);
        GameInput.Instance.SwitchActionMaps();
        currentStationUI = null;
    }

    private void GameInput_OnCloseUI()
    {
        CloseCurrentStationUI();
    }

    private void OnEnable()
    {
        GameInput.Instance.OnCloseUI += GameInput_OnCloseUI;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnCloseUI -= GameInput_OnCloseUI;
    }
}
