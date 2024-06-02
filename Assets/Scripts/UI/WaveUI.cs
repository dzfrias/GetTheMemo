using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private WaveSO currentWave;

    private void OnEnable()
    {
        WaveManager.OnNewWave += NewWave;
    }

    private void OnDisable()
    {
        WaveManager.OnNewWave -= NewWave;
    }

    private void NewWave(WaveSO wave)
    {
        text.gameObject.SetActive(true);
        text.text = $"Wave {wave.waveNumber}";
        currentWave = wave;
    }
}
