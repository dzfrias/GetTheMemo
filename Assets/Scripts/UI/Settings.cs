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
    [SerializeField] private GameObject noBloomPrefab;
    [SerializeField] private Toggle lightsToggle;

    private MMF_Player player;
    private CinemachinePOVExtension playerCameraSettings;
    private bool closing;
    private Resolution[] resolutions;
    private GameObject noBloom;

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

        GameObject playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera");
        if (playerCamera != null)
        {
            playerCameraSettings = playerCamera.GetComponent<CinemachinePOVExtension>();
        }
    }

    private void Start()
    {
        noBloom = Instantiate(noBloomPrefab);
        lightsToggle.isOn = SaveData.Instance.data.lightsMode;
        noBloom.SetActive(!SaveData.Instance.data.lightsMode);
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
        if (player.HasFeedbackStillPlaying() || !IsOpen()) return;
        player.PlayFeedbacksInReverse();
        closing = true;
    }

    public bool IsOpen()
    {
        return transform.GetChild(0).gameObject.activeSelf;
    }

    public void SetVolume(float volume)
    {
        // Volume is not linear, so we use this formula to set the parameter
        mixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }

    public void SetSensitivity(float sensitivity)
    {
        if (playerCameraSettings == null) return;
        playerCameraSettings.SetSensitivity(sensitivity);
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

    public void ToggleLights(bool on)
    {
        SaveData.Instance.data.lightsMode = on;
        SaveData.Instance.Save();
        noBloom.SetActive(!on);
    }

    private void OnPlayerComplete()
    {
        if (closing)
        {
            Toggle(false);
            closing = false;
            player.SetDirectionTopToBottom();
        }
    }

    public void Toggle(bool yes)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(yes);
            if (yes)
            {
                player.PlayFeedbacks();
            }
        }
    }
}
