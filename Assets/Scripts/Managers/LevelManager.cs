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

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private List<StoryBeat> beats;

    private int currentBeat;
    private int currentDay;

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

    public int CurrentDay()
    {
        return currentDay;
    }
}
