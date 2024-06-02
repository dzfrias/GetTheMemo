using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public enum DebugTextInfo
{
    Velocity,
    Speed,
    Framerate,
}

public class DebugText : MonoBehaviour
{
    [SerializeField] private DebugTextInfo debugInfo;

    private TextMeshProUGUI text;
    private CharacterController player;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        player = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        if (debugInfo == DebugTextInfo.Framerate)
        {
            StartCoroutine(DisplayFramerate());
        }
    }

    private void Update()
    {
        switch (debugInfo)
        {
            case DebugTextInfo.Velocity:
                text.text = player.velocity.ToString();
                break;
            case DebugTextInfo.Speed:
                text.text = player.velocity.magnitude.ToString();
                break;
            case DebugTextInfo.Framerate:
                break;
        }
    }

    private IEnumerator DisplayFramerate()
    {
        while (true)
        {
            text.text = ((int)(1f / Time.unscaledDeltaTime)).ToString();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
