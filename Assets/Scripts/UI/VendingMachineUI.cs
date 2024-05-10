using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class VendingMachineUI : MonoBehaviour {
    [SerializeField] private MMF_Player openFeedbacks;
    [SerializeField] private MMF_Player closeFeedbacks;

    public void Show()
    {
        gameObject.SetActive(true);
        openFeedbacks.PlayFeedbacks();
    }

    public void Hide()
    {
        closeFeedbacks.PlayFeedbacks();
    }
}
