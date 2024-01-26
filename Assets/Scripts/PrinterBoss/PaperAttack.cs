using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperAttack : StateMachineBehaviour
{
    [SerializeField] private GameObject paperProjectilePrefab;

    private Transform transform;

    private GameObject target;
    private float maxXVariance = 1f;
    private float maxYVariance = 1f;

    private float timeBetweenPaperShots;
    private float maxTimeBetweenPaperShots = 0.5f;
    private int paperCount = 5;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeBetweenPaperShots = maxTimeBetweenPaperShots;
        Debug.Log("PAPER ATTACK ACTIVE!");
        transform = animator.transform;
        target = GameObject.FindGameObjectWithTag("Player");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (paperCount > 0)
        {
            timeBetweenPaperShots -= Time.deltaTime;
            if (timeBetweenPaperShots <= 0)
            {
                ShootPaper();
                timeBetweenPaperShots = maxTimeBetweenPaperShots;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    private void ShootPaper()
    {
        GameObject paper = Instantiate(paperProjectilePrefab, transform.position, transform.rotation);
        PaperProjectile paperProjectile = paper.GetComponent<PaperProjectile>();
        paperProjectile.SetTargetShootingPosition(GetTargetShootingPosition());
    }

    private Vector3 GetTargetShootingPosition()
    {
        float targetHeight = target.GetComponent<CharacterController>().height;
        Vector3 targetShootingPosition = target.transform.position + Vector3.up * targetHeight/2 + GetTargetVariance();
        return targetShootingPosition;
    }

    private Vector3 GetTargetVariance()
    {
        float xVariance = Random.Range(0, maxXVariance);
        float yVariance = Random.Range(0, maxYVariance);
        Vector3 varientVector = new Vector3 (xVariance, yVariance, 0f);
        return varientVector;
    }
}
