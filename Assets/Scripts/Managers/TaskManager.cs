using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    public event Action<int, ITask> OnTaskAdded;
    public event Action<int> OnTaskCompleted;
    public event Action<int> OnTaskUpdated;

    private Dictionary<int, ITask> tasks = new Dictionary<int, ITask>();
    private int nextTaskId = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There is more than 1 task manager in the scene!");
        }
    }

    // TODO: this should not be here - only here for testing purposes
    private void Start()
    {
        PaperShredderTask paperShredderTask = new PaperShredderTask();
        AddTask(paperShredderTask);
        BookshelfTask bookshelfTask = new BookshelfTask();
        AddTask(bookshelfTask);
    }

    /// <summary>
    /// Adds a task to the active tasks list, returns the id of the task.
    /// </summary>
    public int AddTask(ITask task)
    {
        int id = nextTaskId++;
        tasks.Add(id, task);
        task.Start(id);
        OnTaskAdded?.Invoke(id, task);
        return id;
    }

    public void CompleteTask(int taskId)
    {
        ITask task = tasks[taskId];
        tasks.Remove(taskId);
        task.Complete();
        OnTaskCompleted?.Invoke(taskId);
    }

    public ITask GetTask(int taskId)
    {
        return tasks[taskId];
    }

    public void QueueTaskUpdate(int taskId)
    {
        OnTaskUpdated?.Invoke(taskId);
    }
}
