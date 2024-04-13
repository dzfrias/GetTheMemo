using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private int currentHour = 10;
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
        if (currentWave != null)
        {
            currentHour += wave.lengthHours;
        }
        string meridiem;
        if (currentHour >= 12)
        {
            meridiem = "am";
        }
        else
        {
            meridiem = "pm";
        }
        text.gameObject.SetActive(true);
        text.text = $"{(currentHour == 12 ? 12 : currentHour % 12)}{meridiem}";
        currentWave = wave;
    }
}
