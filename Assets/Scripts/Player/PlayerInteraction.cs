using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interact Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private float maxDistance = 1f;

    private IInteractable interactable;

    private void OnEnable()
    {
        GameInput.Instance.OnInteract += GameInput_OnInteract;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnInteract -= GameInput_OnInteract;
    }

    private void Update()
    {
        InteractDetection();
    }

    private void GameInput_OnInteract()
    {
        Interact();
    }

    private void InteractDetection()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out raycastHit, maxDistance))
        {
            if (raycastHit.collider != null)
            {
                GameObject detectedObject = raycastHit.collider.gameObject;
                if (detectedObject.TryGetComponent(out IInteractable interactable))
                {
                    if (InteractableChanged(interactable))
                    {
                        TryHideInteractableOutline();
                    }

                    this.interactable = interactable;
                    interactable.ShowOutline();
                }
                return;
            }
        }
        TryHideInteractableOutline();
        interactable = null;
    }

    private void Interact()
    {
        if (interactable != null)
        {
            interactable.Interact();
        }
    }

    private bool InteractableChanged(IInteractable newInteractable)
    {
        if (interactable != newInteractable)
        {
            return true;
        }
        return false;
    }
    
    private void TryHideInteractableOutline()
    {
        if (interactable != null)
        {
            interactable.HideOutline();
        }
    }
}
