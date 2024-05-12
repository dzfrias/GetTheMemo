using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Settings settings;

    private MMF_Player player;
    private ActionMap oldMap;

    private void Awake()
    {
        player = GetComponent<MMF_Player>();
    }

    private void OnEnable()
    {
        GameInput.Instance.OnPause += Pause;
        GameInput.Instance.OnCloseUI += Close;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnPause -= Pause;
        GameInput.Instance.OnCloseUI -= Close;
    }

    public void Pause()
    {
        if (player.HasFeedbackStillPlaying()) return;
        oldMap = GameInput.Instance.CurrentActionMap();
        GameInput.Instance.SwitchActionMaps(ActionMap.UI);
        player.SetDirectionTopToBottom();
        player.PlayFeedbacks();
        OfficeManager.Instance.Pause();
        Display(true);
    }

    public void Close()
    {
        StartCoroutine(_Close());
    }

    private IEnumerator _Close()
    {
        // settings.IsOpen() will emulate a stack-based UI like the user would
        // expect.
        if (player.HasFeedbackStillPlaying() || settings.IsOpen()) yield break;
        GameInput.Instance.SwitchActionMaps(oldMap);
        player.SetDirectionBottomToTop();
        player.PlayFeedbacks();
        OfficeManager.Instance.UnPause();
        yield return new WaitWhile(() => player.HasFeedbackStillPlaying());
        Display(false);
    }

    private void Display(bool yes)
    {
        transform.GetChild(0).gameObject.SetActive(yes);
    }
}
