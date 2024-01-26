using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChompAttack : StateMachineBehaviour
{
    private Transform transform;
    private GameObject target;
    private CharacterController targetCharacterController;
    private int secondsInAdvance = 2;
    private Vector3 predictedLocation;
    private Vector3 startLocation;
    private float timeElapsed;
    private float timeToWaitMax = 1f;
    private float timeToWait;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeToWait = timeToWaitMax;
        timeElapsed = 0;
 
        transform = animator.transform;
        target = GameObject.FindGameObjectWithTag("Player");
        targetCharacterController = target.GetComponent<CharacterController>();

        SetLocations();
        FacePredictedEndLocation();
    }

    private void SetLocations()
    {
        startLocation = transform.position;
        predictedLocation = GetTargetPredictedLocation();
    }

    private Vector3 GetTargetPredictedLocation()
    {
        Vector3 velocity = targetCharacterController.velocity; 
        velocity *= secondsInAdvance;
        Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        Vector3 predictedPosition = targetPosition + velocity;
        return predictedPosition;
    }

    private void FacePredictedEndLocation()
    {
        transform.LookAt(predictedLocation);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeToWait -= Time.deltaTime;
        if (timeToWait <= 0)
        {
            MoveTowardPredictedLocation();
        }
    }

    private void MoveTowardPredictedLocation()
    {
        timeElapsed += Time.deltaTime;
        transform.position = Vector3.Lerp(startLocation, predictedLocation, timeElapsed/(secondsInAdvance - timeToWaitMax));
    }
}
