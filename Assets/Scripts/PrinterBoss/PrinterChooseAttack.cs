using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterChooseAttack : StateMachineBehaviour
{
    [SerializeField] private List<string> attackNames;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        string attackName = ChooseRandomAttackName();
        animator.SetTrigger(attackName);
    }

    private string ChooseRandomAttackName()
    {
        int randomIndex = Random.Range(0, attackNames.Count);
        string randomAttackName = attackNames[randomIndex];
        return randomAttackName;
    }
}
