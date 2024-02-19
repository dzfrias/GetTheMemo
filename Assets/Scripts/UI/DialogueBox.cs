using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

class Tokenizer
{
    private CharEnumerator chars;

    public Tokenizer(string text)
    {
        chars = text.GetEnumerator();
    }

    /// <summary>
    /// Separates the input into a list of tokens.
    /// </summary>
    ///
    /// A token is defined as either:
    /// - A character
    /// - A tag (`<b>`, `<color="red">`, etc.)
    public List<string> Tokenize()
    {
        List<string> tokens = new List<string>();

        while (chars.MoveNext())
        {
            switch (chars.Current)
            {
                case '<':
                    StringBuilder sb = new StringBuilder();
                    while (chars.Current != '>')
                    {
                        sb.Append(chars.Current);
                        // EOF case, to prevent infinite loop
                        if (!chars.MoveNext())
                        {
                            break;
                        }
                    }
                    sb.Append('>');
                    tokens.Add(sb.ToString());
                    break;
                default:
                    // Allocating a new `string` for every single character
                    // is not really optimal here... if this causes performance
                    // issues  we can easily fix this with a tagged union-like
                    // data structure (C# doesn't have them built-in
                    // unfortunately)
                    tokens.Add(chars.Current.ToString());
                    break;
            }
        }
        return tokens;
    }
}

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
        // We need to separate the input into a list of tokens. Each token
        // will be displayed after `typeDelay`. This is so we can support
        // TextMeshPro styling tags. For example,
        //   <color="red">red hello
        // Will have the `<color...>` tag as its first token. This is so we
        // can put the color tag out immediately into the TMP text. We don't
        // want to display each character of `<color...>` one by one, it should
        // just go to the TMP text as one contiguous block.
        Tokenizer tokenizer = new Tokenizer(msg);
        List<string> tokens = tokenizer.Tokenize();
        textCoroutine = StartCoroutine(TypeText(tokens));
    }

    private IEnumerator TypeText(List<string> tokens)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var token in tokens)
        {
            sb.Append(token);
            dialogueText.text = sb.ToString();
            // If length > 1, it is a styling token, so no delay needed.
            if (token.Length == 1)
            {
                yield return new WaitForSeconds(typeDelay);
            }
        }
        yield return new WaitForSeconds(stopTime);
        gameObject.SetActive(false);
        textCoroutine = null;
    }
}
