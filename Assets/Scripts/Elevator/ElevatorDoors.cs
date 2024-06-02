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
        WaveManager.OnWavesCompleted += OpenToLeave;
    }

    private void OnDisable()
    {
        WaveManager.OnWavesCompleted -= OpenToLeave;
    }

    public void OpenToLeave()
    {
        animator.SetTrigger("OpenDoors");
        leaveLevelTrigger.SetActive(true);
    }
}
