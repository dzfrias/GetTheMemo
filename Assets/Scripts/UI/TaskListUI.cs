using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListUI : MonoBehaviour
{
    [SerializeField]
    private GameObject taskListItemPrefab;

    private Dictionary<int, TaskCheckboxUI> taskListItems = new Dictionary<int, TaskCheckboxUI>();

    private void Awake()
    {
        TaskManager.Instance.OnTaskAdded += TaskManager_OnTaskAdded;
        TaskManager.Instance.OnTaskCompleted += TaskManager_OnTaskCompleted;
    }

    void TaskManager_OnTaskAdded(int id, ITask task)
    {
        GameObject taskListItem = Instantiate(taskListItemPrefab, transform);
        TaskCheckboxUI taskCheckboxUI = taskListItem.GetComponent<TaskCheckboxUI>();
        taskCheckboxUI.SetTaskText(task.Name());
        taskListItems.Add(id, taskCheckboxUI);
    }

    void TaskManager_OnTaskCompleted(int id)
    {
        taskListItems[id].CompleteTask();
        taskListItems.Remove(id);
    }
}
