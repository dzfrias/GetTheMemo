using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DeleteAudioOnFinish : MonoBehaviour
{
    private void Start()
    {
        AudioSource source = GetComponent<AudioSource>();
        StartCoroutine(DeleteAfter(source.clip.length));
    }

    private IEnumerator DeleteAfter(float length)
    {
        yield return new WaitForSeconds(length);
        Destroy(gameObject);
    }
}
