using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable, IHoverable, IGrabbable
{
    private Outline outline;
    private Rigidbody rb;
    private PaperShredderTask paperShredderTask;
    private int paperShredderTaskId;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        rb = GetComponent<Rigidbody>();
        TaskManager.Instance.OnTaskAdded += TaskManager_OnTaskAdded;
        TaskManager.Instance.OnTaskCompleted += TaskManager_OnTaskCompleted;
    }

    public void Interact()
    {
        Debug.Log("Interacted");
        if (paperShredderTask != null)
        {
            StationUIManager.Instance.Startup(paperShredderTask);
        }
    }

    public void TaskManager_OnTaskAdded(int id, ITask task)
    {
        if (task is not PaperShredderTask)
            return;

        if (paperShredderTask != null)
        {
            Debug.LogError("There is already a paper shredder task!");
            return;
        }

        paperShredderTask = task as PaperShredderTask;
        paperShredderTaskId = id;
    }

    public void TaskManager_OnTaskCompleted(int id)
    {
        if (id != paperShredderTaskId)
            return;
        paperShredderTask = null;
        paperShredderTaskId = -1;
    }

    public void Hover()
    {
        outline.enabled = true;
    }

    public void NoHover()
    {
        outline.enabled = false;
    }

    public void Pickup()
    {
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void Drop()
    {
        rb.useGravity = true;
        rb.velocity *= 2f;
        rb.interpolation = RigidbodyInterpolation.None;
    }
}
