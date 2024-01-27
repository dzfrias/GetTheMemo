using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNightTimeInteraction : MonoBehaviour
{
    [SerializeField] private Transform holdTransform;
    [SerializeField] private float throwPower = 1f;

    private GameObject heldObject;

    private void Awake()
    {
        GameInput.Instance.SwitchActionMaps(ActionMap.PlayerNightTime);
    }

    private void OnEnable()
    {
        GameInput.Instance.OnThrow += GameInput_OnThrow;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnThrow -= GameInput_OnThrow;
    }

    private void GameInput_OnThrow()
    {
        if (heldObject != null)
        {
            Rigidbody heldRb = heldObject.GetComponent<Rigidbody>();
            heldObject.transform.parent = null;
            heldRb.AddForce(transform.forward * throwPower, ForceMode.Impulse);
            heldObject = null;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject hitGameObject = collider.gameObject;
        Debug.Log("COLLIDED WITH OBJECT");
        if (hitGameObject.CompareTag("PickupObject"))
        {
            HoldObject(hitGameObject);
        }
    }

    private void HoldObject(GameObject gameObjectToHold)
    {
        heldObject = gameObjectToHold;
        Rigidbody heldRb = heldObject.GetComponent<Rigidbody>();
        heldObject.transform.position = holdTransform.position;
        heldObject.transform.parent = holdTransform;
    }
}
