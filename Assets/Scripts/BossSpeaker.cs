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

    public void Play(AudioClip clip)
    {
        source.clip = clip;
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
