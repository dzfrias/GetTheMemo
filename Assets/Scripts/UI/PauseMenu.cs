using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Settings settings;

    private MMF_Player player;
    private bool isOpen;
    private ActionMap oldMap;

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
        oldMap = GameInput.Instance.CurrentActionMap();
        GameInput.Instance.SwitchActionMaps(ActionMap.UI);
        player.PlayFeedbacks();
        Time.timeScale = 0;
        Display(true);
    }

    public void Close()
    {
        // settings.IsOpen() will emulate a stack-based UI like the user would
        // expect.
        if (player.HasFeedbackStillPlaying() || settings.IsOpen()) return;
        GameInput.Instance.SwitchActionMaps(oldMap);
        player.PlayFeedbacksInReverse();
        Time.timeScale = 1;
    }

    private void OnPlayerComplete()
    {
        isOpen = !isOpen;
        if (!isOpen)
        {
            Display(false);
            player.SetDirectionTopToBottom();
        }
    }

    private void Display(bool yes)
    {
        transform.GetChild(0).gameObject.SetActive(yes);
    }
}
