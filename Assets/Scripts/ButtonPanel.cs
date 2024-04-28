using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanel : MonoBehaviour, IInteractable
{
    [SerializeField] private float delay = 3f;
    [SerializeField] private DialogueBox dialogueBox;

    public void Interact(Vector3 playerPos)
    {
        StartCoroutine(_GoToCombat());
    }

    private IEnumerator _GoToCombat()
    {
        dialogueBox.DisplayText("Going up.%1 Good luck out there,%0.5 intern.");
        yield return new WaitWhile(() => dialogueBox.IsPlaying());
        yield return new WaitForSeconds(delay);
        GameManager.Instance.LoadScene(1);
    }
}
