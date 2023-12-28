using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookshelf : MonoBehaviour, IInteractable, IHoverable
{
    private Outline outline;
    private BookshelfTask bookshelfTask;
    private int bookshelfTaskId;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        TaskManager.Instance.OnTaskAdded += TaskManager_OnTaskAdded;
        TaskManager.Instance.OnTaskCompleted += TaskManager_OnTaskCompleted;
    }

    public void Interact()
    {
        Debug.Log("Interacted");
        if (bookshelfTask != null)
        {
            StationUIManager.Instance.Startup(bookshelfTask);
        }
    }

    public void TaskManager_OnTaskAdded(int id, ITask task)
    {
        if (task is not BookshelfTask)
            return;

        if (bookshelfTask != null)
        {
            Debug.LogError("There is already a paper shredder task!");
            return;
        }

        bookshelfTask = task as BookshelfTask;
        bookshelfTaskId = id;
    }

    public void TaskManager_OnTaskCompleted(int id)
    {
        if (id != bookshelfTaskId)
            return;
        bookshelfTask = null;
        bookshelfTaskId = -1;
    }

    public void Hover()
    {
        outline.enabled = true;
    }

    public void NoHover()
    {
        outline.enabled = false;
    }
}
