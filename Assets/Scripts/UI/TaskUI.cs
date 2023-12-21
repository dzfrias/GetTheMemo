using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private int taskID;
    [SerializeField] private TaskCheckboxUI taskCheckboxUI;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public int GetTaskID()
    {
        return taskID;
    }
}
