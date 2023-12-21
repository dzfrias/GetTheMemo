using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskChecklistUI : MonoBehaviour
{
    [SerializeField] private List<TaskCheckboxUI> taskCheckboxUIList;

    public static TaskChecklistUI Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There is more than 1 task system in the scene!");
        }
    }

    public void CompleteTask(int id)
    {
        foreach (TaskCheckboxUI taskCheckboxUI in taskCheckboxUIList)
        {
            if (taskCheckboxUI.GetTaskID() == id)
            {
                taskCheckboxUI.CompleteTask();
            }
        }
    }
}
