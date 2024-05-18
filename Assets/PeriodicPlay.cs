using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PeriodicPlay : MonoBehaviour
{
    [SerializeField] private MMF_Player player;
    [SerializeField] private float min = 1f;
    [SerializeField] private float range = 2f;

    private void Start()
    {
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        while (true)
        {
            float time = Random.Range(min, min + range);
            yield return new WaitForSeconds(time);
            player.PlayFeedbacks();
        }
    }
}
