using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class PauseMenu : MonoBehaviour
{
    private MMF_Player player;
    private bool isOpen;

    private void Awake()
    {
        player = GetComponent<MMF_Player>();
    }

    private void OnEnable()
    {
        GameInput.Instance.OnPause += Pause;
        GameInput.Instance.OnCloseUI += Close;
        player.Events.OnComplete.AddListener(OnPlayerComplete);
    }

    private void OnDisable()
    {
        GameInput.Instance.OnPause -= Pause;
        GameInput.Instance.OnCloseUI -= Close;
        player.Events.OnComplete.RemoveListener(OnPlayerComplete);
    }

    public void Pause()
    {
        if (player.HasFeedbackStillPlaying()) return;
        GameInput.Instance.SwitchActionMaps(ActionMap.UI);
        player.PlayFeedbacks();
        Time.timeScale = 0;
        Display(true);
    }

    public void Close()
    {
        if (player.HasFeedbackStillPlaying()) return;
        GameInput.Instance.SwitchActionMaps(ActionMap.Player);
        player.PlayFeedbacks();
        Time.timeScale = 1;
    }

    private void OnPlayerComplete()
    {
        isOpen = !isOpen;
        if (!isOpen)
        {
            Display(false);
        }
    }

    private void Display(bool yes)
    {
        transform.GetChild(0).gameObject.SetActive(yes);
    }
}
