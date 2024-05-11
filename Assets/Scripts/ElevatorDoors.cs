using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoors : MonoBehaviour
{
    [SerializeField] private GameObject leaveLevelTrigger;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        WaveManager.OnWavesCompleted += OpenDoors;
        WaveManager.OnWavesCompleted += ActivateLeaveLevelTrigger;
    }

    private void OnDisable()
    {
        WaveManager.OnWavesCompleted -= OpenDoors;
        WaveManager.OnWavesCompleted -= ActivateLeaveLevelTrigger;
    }

    public void OpenDoors()
    {
        animator.SetTrigger("OpenDoors");
    }

    private void ActivateLeaveLevelTrigger()
    {
        leaveLevelTrigger.SetActive(true);
    }
}
