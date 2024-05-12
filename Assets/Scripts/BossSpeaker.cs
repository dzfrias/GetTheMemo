using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpeaker : MonoBehaviour
{
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        OfficeManager.Instance.OnPause += Pause;
        OfficeManager.Instance.OnUnPause += Resume;
    }

    private void OnDisable()
    {
        OfficeManager.Instance.OnPause -= Pause;
        OfficeManager.Instance.OnUnPause -= Resume;
    }

    public void Play(AudioClip clip, float volume)
    {
        source.clip = clip;
        source.volume = volume;
        source.Play();
    }

    public void Pause()
    {
        source.Pause();
    }

    public void Resume()
    {
        source.Play();
    }

    public float Time()
    {
        return source.time;
    }

    public float TimeLeft()
    {
        return source.clip.length - source.time;
    }

    public bool IsPlaying()
    {
        return source.isPlaying;
    }
}
