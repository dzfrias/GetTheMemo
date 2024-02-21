using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable, IRecordMode
{
    private Animator animator;

    private Vector3 forwardDirection;
    private bool isOpen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        forwardDirection = transform.forward;
    }

    public void Interact(Vector3 playerPosition)
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open(playerPosition);
        }
    }

    public void OnEnterRecordMode()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 70, transform.rotation.eulerAngles.z);
    }

    private void Close()
    {
        animator.SetTrigger("CloseDoor");
        isOpen = false;
    }

    private void Open(Vector3 playerPosition)
    {
        float direction = Vector3.Dot(forwardDirection, (playerPosition - transform.position).normalized);
        Debug.Log(direction);
        if (direction > 0)
        {
            animator.SetTrigger("OpenDoorForward");
        }
        else
        {
            animator.SetTrigger("OpenDoorBackward");
        }
        isOpen = true;
    }
}
