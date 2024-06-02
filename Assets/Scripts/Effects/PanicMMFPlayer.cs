using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

[RequireComponent(typeof(MMF_Player))]
public class PanicMMFPlayer : MonoBehaviour
{
    private MMF_Player player;
    private Panic playerPanic;

    private void Awake()
    {
        player = GetComponent<MMF_Player>();
        playerPanic = GameObject.FindGameObjectWithTag("Player").GetComponent<Panic>();
    }

    private void OnEnable()
    {
        playerPanic.OnPanicStart += Change;
        playerPanic.OnPanicEnd += Restore;
    }

    private void OnDisable()
    {
        playerPanic.OnPanicStart -= Change;
        playerPanic.OnPanicEnd -= Restore;
    }

    private void Change()
    {
        player.SetDirectionTopToBottom();
        player.PlayFeedbacks();
    }

    private void Restore()
    {
        player.SetDirectionBottomToTop();
        player.PlayFeedbacks();
    }
}
