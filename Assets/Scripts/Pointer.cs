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
        GameInput.Instance.OnOpenUI += GameInput_OnOpenUI;
        GameInput.Instance.OnCloseUISuccess += GameInput_OnCloseUISuccess;
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnOpenUI -= GameInput_OnOpenUI;
        GameInput.Instance.OnCloseUISuccess -= GameInput_OnCloseUISuccess;
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

    public void GameInput_OnOpenUI()
    {
        gameObject.SetActive(false);
    }

    public void GameInput_OnCloseUISuccess()
    {
        gameObject.SetActive(true);
    }
}
