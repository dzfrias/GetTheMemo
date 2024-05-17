using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[System.Serializable]
public class StoryBeat
{
    public float autoContinue = -1;
    public UnityEvent onStart;
    public UnityEvent onEnd;
}

[System.Serializable]
public class LevelBeats
{
    public List<StoryBeat> content;
}

public class OfficeManager : MonoBehaviour, IRecordMode
{
    public static OfficeManager Instance;
    public event Action OnPause;
    public event Action OnUnPause;

    [SerializeField] private bool devMode;
    [SerializeField] private int startOn;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private TutorialText tutorialText;
    [SerializeField] private List<StoryBeat> beats;
    [SerializeField] private List<LevelBeats> perLevelBeats;
    [SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private BossSpeaker speaker;

    private int currentBeat;
    private List<StoryBeat> currentBeats;

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
        int level = SaveData.Instance.data.currentLevel;
        if (level == 0 || perLevelBeats.Count == 0)
        {
            currentBeats = beats;
        }
        else
        {
            currentBeats = perLevelBeats[level - 1].content;
        }
        currentBeat = startOn;
        if (playOnStart)
        {
            NextBeat();
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int level)
    {
        SaveData.Instance.Save();
        SceneManager.LoadScene(level);
    }

    public void NextBeat()
    {
        if (devMode) return;
        if (currentBeat != 0)
        {
            currentBeats[currentBeat - 1].onEnd?.Invoke();
        }
        if (currentBeat >= currentBeats.Count)
        {
            Debug.LogWarning("Attempting to go to story beat that does not exist");
            return;
        }
        var current = currentBeats[currentBeat];
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

    public void OnEnterRecordMode()
    {
        devMode = true;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        OnPause?.Invoke();
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        OnUnPause?.Invoke();
    }

    public void CompleteLevel()
    {
        SaveData.Instance.data.currentLevel += 1;
    }

    private IEnumerator _PlayDialogueSequence(DialogueSO dialogueList)
    {
        yield return new WaitForSeconds(dialogueList.preDelay);
        if (dialogueList.clip != null)
        {
            speaker.Play(dialogueList.clip, dialogueList.volume);
        }
        int i = 0;
        foreach (var dialogue in dialogueList.dialogues)
        {
            yield return new WaitForSeconds(dialogue.timestamp - speaker.Time());
            if (dialogueBox.IsPlaying())
            {
                speaker.Pause();
                yield return new WaitWhile(() => dialogueBox.IsPlaying());
                speaker.Resume();
            }
            dialogueBox.DisplayText(dialogue.text, -1);
            i += 1;
        }
        yield return new WaitWhile(() => dialogueBox.IsPlaying() || speaker.IsPlaying());
        dialogueBox.Hide();
        if (dialogueList.autoContinue)
        {
            NextBeat();
        }
    }

    private IEnumerator AutoContinue(float time)
    {
        yield return new WaitForSeconds(time);
        NextBeat();
    }
}
