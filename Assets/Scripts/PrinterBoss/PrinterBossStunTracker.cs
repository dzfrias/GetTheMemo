using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterBossStunTracker : MonoBehaviour
{
    [SerializeField] private int hitAmountMax;

    private Animator animator;
    private int hitAmount;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void IncreaseStunAmount()
    {
        hitAmount += 1;
        if (hitAmount >= hitAmountMax)
        {
            animator.SetTrigger("Stun");
            hitAmount = 0;
            Debug.Log("STUNNED");
        }
    }
}
