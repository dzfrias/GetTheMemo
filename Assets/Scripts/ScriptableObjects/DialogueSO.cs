using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string text;
    public float timestamp;
}

[CreateAssetMenu(fileName = "DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public float preDelay;
    public AudioClip clip;
    public List<Dialogue> dialogues;
    public bool autoContinue;
    public float volume = 1f;
}
