using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private int taskID;
    [SerializeField] private GameObject checkmarkImage;

    public void CompleteTask()
    {
        checkmarkImage.SetActive(true);
    }

    public int GetTaskID()
    {
        return taskID;
    }
}
