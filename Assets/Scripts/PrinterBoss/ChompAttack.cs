using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChompAttack : StateMachineBehaviour
{
    [SerializeField] private float damage = 5f;

    private Transform transform;
    private BoxCollider boxCollider;
    private GameObject target;
    private CharacterController targetCharacterController;

    private List<GameObject> hitGameObjects;
    private int secondsInAdvance = 2;
    private Vector3 predictedLocation;
    private Vector3 startLocation;
    private float timeElapsed;
    private float timeToWaitMax = 1f;
    private float timeToWait;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hitGameObjects = new();
        timeToWait = timeToWaitMax;
        timeElapsed = 0;
 
        transform = animator.transform;
        boxCollider = transform.GetComponent<BoxCollider>();
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
            DamageHitObjects();
        }
    }

    private void MoveTowardPredictedLocation()
    {
        timeElapsed += Time.deltaTime;
        transform.position = Vector3.Lerp(startLocation, predictedLocation, timeElapsed/(secondsInAdvance - timeToWaitMax));
    }

    private void DamageHitObjects()
    {
        RaycastHit[] raycastHits = GetHitObjects();
        foreach (RaycastHit raycastHit in raycastHits)
        {
            GameObject hitGameObject = raycastHit.collider.gameObject;
            if (IsDamageableObject(hitGameObject))
            {
                Health hitObjectHealth = hitGameObject.GetComponent<Health>();
                hitObjectHealth.TakeDamage(damage);
                hitGameObjects.Add(hitGameObject);
            }
        }
    }

    private RaycastHit[] GetHitObjects()
    {
        Vector3 hitbox = new Vector3(boxCollider.size.x, boxCollider.size.y, 0f);
        RaycastHit[] raycastHits = Physics.BoxCastAll(transform.position, hitbox/2, transform.forward, transform.rotation, boxCollider.size.z);
        return raycastHits;
    }

    private bool IsDamageableObject(GameObject hitGameObject)
    {
        if (hitGameObject.transform != transform 
        && !hitGameObjects.Contains(hitGameObject)
        && hitGameObject.TryGetComponent(out Health _))
        {
            return true;
        }
        return false;
    }
}
