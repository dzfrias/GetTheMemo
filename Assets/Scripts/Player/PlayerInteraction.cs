using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interact Settings")]
    [SerializeField] private float maxDistance = 1f;
    [SerializeField] private Pointer pointer;
    private Camera cam;

    [Header("Pickup Settings")]
    [SerializeField] private Transform holdTransform;

    private GameObject hoverable;
    private IGrabbable heldObject;
    private PlayerHold playerHold;

    private void Awake()
    {
        cam = Camera.main;
        playerHold = GetComponent<PlayerHold>();
    }

    private void OnEnable()
    {
        GameInput.Instance.OnInteract += GameInput_OnInteract;
        GameInput.Instance.OnPickup += GameInput_OnPickup;
        GameInput.Instance.OnDrop += GameInput_OnDrop;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnInteract -= GameInput_OnInteract;
        GameInput.Instance.OnPickup -= GameInput_OnPickup;
        GameInput.Instance.OnDrop -= GameInput_OnDrop;
    }

    private void Update()
    {
        OutlineDetection();
    }

    private void GameInput_OnInteract()
    {
        Interact();
    }

    private void GameInput_OnPickup()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, maxDistance) && raycastHit.collider != null)
        {
            GameObject detectedObject = raycastHit.collider.gameObject;
            if (detectedObject.TryGetComponent(out IGrabbable grabbable))
            {
                grabbable.Pickup();
                heldObject = grabbable;
                playerHold.CreateAnchorPoint(detectedObject);
            }
        }
    }

    private void GameInput_OnDrop()
    {
        if (heldObject != null)
        {
            heldObject.Drop();
            playerHold.DestroyAnchorPoint();
        }
    }

    private void OutlineDetection()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit raycastHit;
        // Checks if the object we're looking at (in range) is interactable
        if (Physics.Raycast(ray, out raycastHit, maxDistance)
                && raycastHit.collider != null
                && raycastHit.collider.gameObject.TryGetComponent(out IInteractable newHoverable))
        {
            GameObject detectedObject = raycastHit.collider.gameObject;
            // Case 1: We're hovering over the same object
            if (hoverable == detectedObject)
            {
                return;
            }
            // Case 2: We're hovering over a different object, try to unhover
            // the current one (falls through to). Then hover the new object.
            if (hoverable != detectedObject)
            {
                TryNoHover();
            }
            hoverable = detectedObject;
            detectedObject.layer = LayerMask.NameToLayer("Outline");
            pointer.OnHover();
        }
        else
        {
            if (hoverable != null)
            {
                // Only called when an object is newly unhovered
                pointer.Reset();
            }
            TryNoHover();
            hoverable = null;
        }
    }

    private void Interact()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, maxDistance) && raycastHit.collider != null)
        {
            GameObject detectedObject = raycastHit.collider.gameObject;
            if (detectedObject.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact(transform.position);
            }
        }
    }

    private void TryNoHover()
    {
        if (hoverable == null) return;
        hoverable.layer = 0;
    }
}
