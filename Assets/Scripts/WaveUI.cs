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
        currentHour %= 24;
        int display;
        string designator;
        if (currentHour == 0)
        {
            designator = "am";
            display = 12;
        }
        else if (currentHour >= 12)
        {
            designator = "pm";
            display = currentHour == 12 ? 12 : currentHour % 12;
        }
        else
        {
            designator = "am";
            display = currentHour;
        }
        text.gameObject.SetActive(true);
        text.text = $"{display}{designator}";
        currentWave = wave;
    }
}
