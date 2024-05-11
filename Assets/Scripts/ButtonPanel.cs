using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class ButtonPanel : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueSO dialogue;
    [SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private GameObject openTrigger;

    private MMF_Player player;
    private bool didInteract;

    private void Start()
    {
        player = GetComponent<MMF_Player>();
    }

    public void Interact(Vector3 playerPos)
    {
        if (didInteract) return;
        didInteract = true;
        GoToCombat();
    }

    public void _Finish()
    {
        int nextLevel = SaveData.Instance.data.currentLevel + 1;
        OfficeManager.Instance.LoadScene(1);
    }

    private void GoToCombat()
    {
        OfficeManager.Instance.PlayDialogueSequence(dialogue);
        openTrigger.SetActive(false);
        player.PlayFeedbacks();
    }
}
