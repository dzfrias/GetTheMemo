using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StoryBeat
{
    public UnityEvent onStart;
    public UnityEvent onEnd;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private List<StoryBeat> beats;
    [SerializeField] private DialogueBox dialogueBox;

    private int currentBeat;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There is more than 1 level manager in the scene!");
        }
    }

    private void Start()
    {
        NextBeat();
    }

    public void NextBeat()
    {
        if (currentBeat != 0)
        {
            beats[currentBeat - 1].onEnd?.Invoke();
        }
        if (currentBeat >= beats.Count)
        {
            Debug.LogWarning("Attempting to go to story beat that does not exist");
            return;
        }
        beats[currentBeat].onStart?.Invoke();
        currentBeat += 1;
    }

    public void PlayDialogueSequence(DialogueSO dialogueList)
    {
        StartCoroutine(_PlayDialogueSequence(dialogueList));
    }

    private IEnumerator _PlayDialogueSequence(DialogueSO dialogueList)
    {
        foreach (var dialogue in dialogueList.dialogues)
        {
            yield return new WaitWhile(() => dialogueBox.IsPlaying());
            yield return new WaitForSeconds(dialogue.preDelay);
            dialogueBox.DisplayText(dialogue.text);
        }
    }
}
