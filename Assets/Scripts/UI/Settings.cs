using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private TMP_Dropdown resolutionsDropdown;

    private MMF_Player player;
    private bool closing;
    private Resolution[] resolutions;

    private void Awake()
    {
        player = GetComponent<MMF_Player>();
        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();
        var options = new List<string>();
        int current = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                current = i;
            }
        }
        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = current;
        resolutionsDropdown.RefreshShownValue();
    }

    private void OnEnable()
    {
        GameInput.Instance.OnCloseUI += Close;
        player.Events.OnComplete.AddListener(OnPlayerComplete);
    }

    private void OnDisable()
    {
        GameInput.Instance.OnCloseUI -= Close;
        player.Events.OnComplete.RemoveListener(OnPlayerComplete);
    }

    public void Close()
    {
        if (player.HasFeedbackStillPlaying()) return;
        player.PlayFeedbacksInReverse();
        closing = true;
    }

    public bool IsOpen()
    {
        return gameObject.activeSelf;
    }

    public void SetVolume(float volume)
    {
        // Volume is not linear, so we use this formula to set the parameter
        mixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    public void SetResolution(int i)
    {
        Resolution resolution = resolutions[i];
        Screen.SetResolution(resolution.width, resolution.height, false);
    }

    private void OnPlayerComplete()
    {
        if (closing)
        {
            gameObject.SetActive(false);
            closing = false;
            player.SetDirectionTopToBottom();
        }
    }
}
