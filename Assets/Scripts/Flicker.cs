using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Flicker : MonoBehaviour
{
    [SerializeField] private float minInterval = 5f;
    [SerializeField] [Range(0f, 10f)] private float intervalRange = 1f;
    [SerializeField] [Range(0f, 1f)] private float twiceProbability;

    private Light light;

    private void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(AutoFlicker());
    }

    private IEnumerator AutoFlicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(minInterval + Random.Range(0f, intervalRange));
            int times = Random.value <= twiceProbability ? 2 : 1;
            for (int i = 0; i < times; i++)
            {
                light.enabled = false;
                yield return new WaitForSeconds(0.1f);
                light.enabled = true;
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
