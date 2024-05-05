using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class ButtonPanel : MonoBehaviour, IInteractable
{
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
        StartCoroutine(_GoToCombat());
    }

    public void _Finish()
    {
        GameManager.Instance.LoadScene(1);
    }

    private IEnumerator _GoToCombat()
    {
        dialogueBox.DisplayText("Going up.%1 Good luck out there,%0.5 intern.");
        openTrigger.SetActive(false);
        yield return new WaitWhile(() => dialogueBox.IsPlaying());
        player.PlayFeedbacks();
    }
}
