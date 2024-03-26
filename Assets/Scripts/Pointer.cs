using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class Pointer : MonoBehaviour
{
    private MMF_Player player;

    private void Start()
    {
        player = GetComponent<MMF_Player>();
    }

    public void OnHover()
    {
        player.SetDirectionTopToBottom();
        player.PlayFeedbacks();
    }

    public void Reset()
    {
        player.SetDirectionBottomToTop();
        if (!player.HasFeedbackStillPlaying())
        {
            player.PlayFeedbacks();
        }
    }
}
