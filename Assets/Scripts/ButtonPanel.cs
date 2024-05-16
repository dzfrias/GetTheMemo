using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

public class ButtonPanel : MonoBehaviour, IInteractable
{
    private enum Destination
    {
        Office,
        Combat,
    }

    [SerializeField] private DialogueSO dialogue;
    [SerializeField] private DialogueSO downDialogue;
    [SerializeField] private UnityEvent onTrigger;

    private MMF_Player player;
    private bool didInteract;
    private Destination dest;

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
        int nextLevel = dest == Destination.Combat ? SaveData.Instance.data.currentLevel + 1 : 0;
        OfficeManager.Instance.LoadScene(nextLevel);
    }

    public void GoToCombat()
    {
        OfficeManager.Instance.PlayDialogueSequence(dialogue);
        onTrigger?.Invoke();
        player.PlayFeedbacks();
        dest = Destination.Combat;
    }

    public void GoToOffice()
    {
        OfficeManager.Instance.PlayDialogueSequence(downDialogue);
        player.PlayFeedbacks();
        dest = Destination.Office;
    }
}
