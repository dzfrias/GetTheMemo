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
            if (child.GetComponent<IStationUI<T>>() == null)
                continue;
            child.gameObject.SetActive(true);
            IStationUI<T> station = child.GetComponent<IStationUI<T>>();
            station.Startup(data);
            currentStationUI = child.gameObject;
            GameInput.Instance.SwitchActionMaps(station.PreferredActionMap());
            return;
        }

        Debug.LogError($"station for {data.GetType()} not found!");
    }

    private void CloseCurrentStationUI()
    {
        if (currentStationUI == null) return;
        currentStationUI.SetActive(false);
        GameInput.Instance.SwitchActionMaps(ActionMap.Player);
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
