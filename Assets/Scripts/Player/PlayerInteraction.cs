using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interact Settings")]
    [SerializeField] private float maxDistance = 1f;
    private Camera cam;

    [Header("Pickup Settings")]
    [SerializeField] private Transform holdTransform;
    
    private IHoverable hoverable;
    private IGrabbable heldObject;

    private void Awake()
    {
        cam = Camera.main;
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
                detectedObject.transform.position = holdTransform.position;
                grabbable.Pickup(holdTransform);
                heldObject = grabbable;
            }   
        }
    }

    private void GameInput_OnDrop()
    {
        if (heldObject != null)
        {
            heldObject.Drop();
        }
    }

    private void OutlineDetection()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, maxDistance) && raycastHit.collider != null)
        {
            GameObject detectedObject = raycastHit.collider.gameObject;
            if (detectedObject.TryGetComponent(out IHoverable newHoverable))
            {
                if (hoverable != newHoverable)
                {
                    TryNoHover();
                }

                hoverable = newHoverable;
                hoverable.Hover();
                return;
            }
        }
        TryNoHover();
        hoverable = null;
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
                interactable.Interact();
            }   
        }
    }

    private void TryNoHover()
    {
        if (hoverable == null) return;
        hoverable.NoHover();
    }
}
