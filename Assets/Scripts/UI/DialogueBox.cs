using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private float typeDelay = 0.2f;
    [SerializeField] private float stopTime = 1f;

    private Coroutine textCoroutine;

    public void DisplayText(string msg)
    {
        if (textCoroutine is not null)
        {
            StopCoroutine(textCoroutine);
        }
        gameObject.SetActive(true);
        textCoroutine = StartCoroutine(TypeText(msg));
    }

    private IEnumerator TypeText(string msg)
    {
        for (int current = 0; current <= msg.Length; current++)
        {
            dialogueText.text = msg.Substring(0, current);
            yield return new WaitForSeconds(typeDelay);
        }
        yield return new WaitForSeconds(stopTime);
        gameObject.SetActive(false);
        textCoroutine = null;
    }
}
