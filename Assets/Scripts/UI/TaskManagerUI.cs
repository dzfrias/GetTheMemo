using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManagerUI : MonoBehaviour
{
    public static TaskManagerUI Instance;

    [SerializeField] private List<TaskUI> taskUIList;

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

    public void StartTask(int id)
    {
        foreach (TaskUI taskUI in taskUIList)
        {
            if (taskUI.GetTaskID() == id)
            {
                taskUI.Show();
            }
        }
    }
}
