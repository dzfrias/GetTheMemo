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
            heldObject.transform.parent = null;
            ThrowableObject throwableObject = heldObject.GetComponent<ThrowableObject>();
            throwableObject.Throw(transform.forward, throwPower);
            heldObject = null;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject hitGameObject = collider.gameObject;
        if (hitGameObject.CompareTag("PickupObject") && !IsHoldingThrowableObject())
        {
            HoldObject(hitGameObject);
        }
    }

    private bool IsHoldingThrowableObject()
    {
        if (holdTransform.childCount == 0)
        {
            return false;
        }
        return true;
    }

    private void HoldObject(GameObject gameObjectToHold)
    {
        heldObject = gameObjectToHold;
        heldObject.transform.position = holdTransform.position;
        heldObject.transform.parent = holdTransform;
    }
}
