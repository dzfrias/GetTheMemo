using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StoryBeat
{
    public float autoContinue = -1;
    public UnityEvent onStart;
    public UnityEvent onEnd;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private bool devMode;
    [SerializeField] private TutorialText tutorialText;
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
        if (devMode) return;
        if (currentBeat != 0)
        {
            beats[currentBeat - 1].onEnd?.Invoke();
        }
        if (currentBeat >= beats.Count)
        {
            Debug.LogWarning("Attempting to go to story beat that does not exist");
            return;
        }
        var current = beats[currentBeat];
        current.onStart?.Invoke();
        currentBeat += 1;
        if (current.autoContinue > 0)
        {
            StartCoroutine(AutoContinue(current.autoContinue));
        }
    }

    public void PlayDialogueSequence(DialogueSO dialogueList)
    {
        StartCoroutine(_PlayDialogueSequence(dialogueList));
    }

    public void DisplayTutorial(string text)
    {
        tutorialText.Display(text);
    }

    public void HideTutorial()
    {
        tutorialText.Hide();
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

    private IEnumerator AutoContinue(float time)
    {
        yield return new WaitForSeconds(time);
        NextBeat();
    }
}
