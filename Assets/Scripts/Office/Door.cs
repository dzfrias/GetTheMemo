using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class Door : MonoBehaviour, IInteractable, IRecordMode
{
    [SerializeField] private bool openOnRecord = true;

    private MMF_Player player;
    private MMF_Rotation openRotation;
    private MMF_Rotation closeRotation;

    private Vector3 forwardDirection;
    private bool isOpen = false;

    private void Awake()
    {
        player = GetComponent<MMF_Player>();
        List<MMF_Rotation> rotationFeedbacks = player.GetFeedbacksOfType<MMF_Rotation>();
        if (rotationFeedbacks.Count != 2)
        {
            Debug.LogError("MMF_Player should have two rotation feedbacks");
        }
        openRotation = rotationFeedbacks[0];
        closeRotation = rotationFeedbacks[1];

        forwardDirection = transform.forward;
    }

    public void Interact(Vector3 playerPosition)
    {
        if (player.HasFeedbackStillPlaying()) return;

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
        if (!openOnRecord) return;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 70, transform.rotation.eulerAngles.z);
    }

    private void Close()
    {
        player.ResumeFeedbacks();
        isOpen = false;
    }

    private void Open(Vector3 playerPosition)
    {
        float direction = Vector3.Dot(forwardDirection, (playerPosition - transform.position).normalized);
        if (direction > 0)
        {
            openRotation.RemapCurveOne = -90f;
            closeRotation.RemapCurveOne = 90f;
        }
        else
        {
            openRotation.RemapCurveOne = 90f;
            closeRotation.RemapCurveOne = -90f;
        }
        player.PlayFeedbacks();
        isOpen = true;
    }
}
