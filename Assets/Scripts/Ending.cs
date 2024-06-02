using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField] private GameObject endingUI;
    [SerializeField] private DialogueSO dialogue;
    [SerializeField] private DialogueSO dialogue2;
    [SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private BossSpeaker bossSpeaker;
    [SerializeField] private DialogueSO yesDialogue;
    [SerializeField] private DialogueSO noDialogue;
    [SerializeField] private GameObject endingPanel;

    private void OnEnable()
    {
        WaveManager.OnWavesCompleted += StartEnd;
    }

    private void OnDisable()
    {
        WaveManager.OnWavesCompleted -= StartEnd;
    }

    private void StartEnd()
    {
        StartCoroutine(_StartEnd());
    }

    private IEnumerator _StartEnd()
    {
        OfficeManager.Instance.PlayDialogueSequence(dialogue);
        yield return new WaitForSeconds(12f);
        OfficeManager.Instance.PlayDialogueSequence(dialogue2);
        yield return new WaitForSeconds(12f);
        endingUI.SetActive(true);
        GameInput.Instance.SwitchActionMaps(ActionMap.UI);
    }

    public void End(bool yes)
    {
        StartCoroutine(_End(yes));
    }

    private IEnumerator _End(bool yes)
    {
        endingUI.SetActive(false);
        DialogueSO toPlay = yes ? yesDialogue : noDialogue;
        OfficeManager.Instance.PlayDialogueSequence(toPlay);
        yield return new WaitForSeconds(10f);
        endingPanel.SetActive(true);
    }
}
