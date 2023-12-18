using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private List<Sound> sounds;

    [Serializable]
    public struct Sound
    {
        public string name;
        public AudioClip audioClip;
        public float volume;
        public float pitch;
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
    }

    public void PlaySound(string name, Vector3 location)
    {
        bool foundSound = false;
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                AudioSource.PlayClipAtPoint(sound.audioClip, location);
                foundSound = true;
            }
        }

        if (!foundSound)
        {
            Debug.LogError($"No Audio Clip Found with Name: '{name}'");
        }
    }
}
