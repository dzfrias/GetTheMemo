using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSystemUI : MonoBehaviour
{
    [SerializeField] private List<TaskUI> taskUIList;

    public static TaskSystemUI Instance;

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
        foreach (TaskUI taskUI in taskUIList)
        {
            if (taskUI.GetTaskID() == id)
            {
                taskUI.CompleteTask();
            }
        }
    }
}
