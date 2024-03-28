using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string text;
    public float preDelay;
}

[CreateAssetMenu(fileName = "DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public List<Dialogue> dialogues;
}
