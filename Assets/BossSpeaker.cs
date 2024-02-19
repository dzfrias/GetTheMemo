using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpeaker : MonoBehaviour
{
    [SerializeField] private AnimationCurve soundProbabilityCurve;
    [SerializeField] private bool playOnStart;

    private AudioSource source;
    private Coroutine playCoroutine;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        if (playOnStart)
        {
            StartPlaying();
        }
    }

    public void StartPlaying()
    {
        playCoroutine = StartCoroutine(PlayCoroutine());
    }

    public void StopPlaying()
    {
        StopCoroutine(playCoroutine);
        playCoroutine = null;
    }

    private IEnumerator PlayCoroutine()
    {
        while (true)
        {
            float volume = Random.Range(0.4f, 1f);
            float pitch = soundProbabilityCurve.Evaluate(Random.value);
            float newLength = source.clip.length / System.Math.Abs(pitch);
            source.pitch = pitch;
            source.volume = volume;
            source.Play();
            yield return new WaitForSeconds(newLength);
        }
    }
}
