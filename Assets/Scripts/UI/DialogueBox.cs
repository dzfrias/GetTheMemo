using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

abstract class Token {}

class StringToken : Token
{
    public string text { get; }

    public StringToken(string text)
    {
        this.text = text;
    }

    public override string ToString()
    {
        return text;
    }
}

class WaitToken : Token
{
    public float waitTime { get; }
    public char ate { get; }

    public WaitToken(float value, char ate)
    {
        waitTime = value;
        this.ate = ate;
    }
}

class StylingToken : Token
{
    public string text { get; }

    public StylingToken(string text)
    {
        this.text = text;
    }

    public override string ToString()
    {
        return text;
    }
}

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
    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (chars.MoveNext())
        {
            switch (chars.Current)
            {
                case '<':
                    var sb = new StringBuilder();
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
                    tokens.Add(new StylingToken(sb.ToString()));
                    break;
                case '%':
                    var waitSb = new StringBuilder();
                    chars.MoveNext();
                    while (Char.IsDigit(chars.Current) || chars.Current == '.')
                    {
                        waitSb.Append(chars.Current);
                        if (!chars.MoveNext())
                        {
                            break;
                        }
                    }
                    var waitTime = float.Parse(waitSb.ToString());
                    tokens.Add(new WaitToken(waitTime, chars.Current));
                    break;
                default:
                    // Allocating a new `string` for every single character
                    // is not really optimal here... if this causes performance
                    // issues  we can easily fix this with a tagged union-like
                    // data structure (C# doesn't have them built-in
                    // unfortunately)
                    tokens.Add(new StringToken(chars.Current.ToString()));
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

    [SerializeField] private UnityEvent onStart;
    [SerializeField] private UnityEvent onEnd;

    private Coroutine textCoroutine;

    public void DisplayText(string msg)
    {
        DisplayText(msg, stopTime);
    }

    public void DisplayText(string msg, float overrideStopTime)
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
        List<Token> tokens = tokenizer.Tokenize();
        textCoroutine = StartCoroutine(TypeText(tokens, overrideStopTime));
        onStart?.Invoke();
    }

    public bool IsPlaying()
    {
        return textCoroutine != null;
    }

    private IEnumerator TypeText(List<Token> tokens, float overrideStopTime)
    {
        var sb = new StringBuilder();
        foreach (var token in tokens)
        {
            if (token is WaitToken)
            {
                var waitToken = (WaitToken)token;
                sb.Append(waitToken.ate);
                yield return new WaitForSeconds(waitToken.waitTime);
                continue;
            }
            string tokenText = token.ToString();
            sb.Append(tokenText);
            dialogueText.text = sb.ToString();
            // If length > 1, it is a styling token, so no delay needed.
            if (token is StringToken)
            {
                yield return new WaitForSeconds(typeDelay);
            }
        }
        onEnd?.Invoke();
        yield return new WaitForSeconds(overrideStopTime);
        if (overrideStopTime > 0)
        {
            gameObject.SetActive(false);
        }
        textCoroutine = null;
    }
}
