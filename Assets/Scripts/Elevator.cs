using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private GameObject entryPrevention;

    private void OnEnable()
    {
        WaveManager.OnWavesCompleted += DisableEntryPrevention;
    }

    private void OnDisable()
    {
        WaveManager.OnWavesCompleted -= DisableEntryPrevention;
    }

    private void DisableEntryPrevention()
    {
        entryPrevention.SetActive(false);
    }
}
