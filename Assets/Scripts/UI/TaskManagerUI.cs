using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManagerUI : MonoBehaviour
{
    public static TaskManagerUI Instance;

    [SerializeField] private List<TaskUI> taskUIList;

    private TaskUI currentTaskUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There is more than 1 task manager ui in the scene!");
        }
    }

    private void OnEnable()
    {
        GameInput.Instance.OnCloseUI += GameInput_OnCloseUI;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnCloseUI -= GameInput_OnCloseUI;
    }

    private void GameInput_OnCloseUI()
    {
        if (currentTaskUI)
        {
            StopCurrentTask();
        }
    }

    public void StartTask(int id)
    {
        TaskUI taskUI = FindTask(id);
        if (taskUI)
        {
            taskUI.Show();
            GameInput.Instance.SwitchActionMaps();
            currentTaskUI = taskUI;
        }
    }

    public void StopCurrentTask()
    {
        currentTaskUI.Hide();
        GameInput.Instance.SwitchActionMaps();
    }

    private TaskUI FindTask(int id)
    {
        foreach (TaskUI taskUI in taskUIList)
        {
            if (taskUI.GetTaskID() == id)
            {
                return taskUI;
            }
        }
        return null;
    }
}
