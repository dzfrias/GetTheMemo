using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private List<Sound> music;
    [SerializeField] private List<Sound> soundEffects;

    private AudioSource audioSource;

    [Serializable]
    public struct Sound
    {
        public string name;
        public AudioClip audioClip;
        public float volume;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogError("There is more than 1 sound manager in the scene!");
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string name)
    {
        Sound sound = GetSound(name, soundEffects);
        GameObject gameObject = new GameObject("Sound");
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sound.audioClip;
        audioSource.volume = sound.volume;
        audioSource.Play();
        StartCoroutine(DestroySound(audioSource));
    }

    public void SetMusic(string name)
    {
        Sound sound = GetSound(name, music);
        audioSource.clip = sound.audioClip;
        audioSource.volume = sound.volume;
        audioSource.Play();
    }

    private Sound GetSound(string name, List<Sound> soundList)
    {
        foreach (Sound sound in soundList)
        {
            if (sound.name == name)
            {
                return sound;
            }
        }
        Debug.LogError($"No Audio Clip Found with Name: '{name}'");
        return soundList[0];
    }

    private IEnumerator DestroySound(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(audioSource.gameObject);
    }
}
