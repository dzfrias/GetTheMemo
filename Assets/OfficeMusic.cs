using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeMusic : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;

    private void Start()
    {
        audioManager.PlayOfficeMusic();
    }
}
