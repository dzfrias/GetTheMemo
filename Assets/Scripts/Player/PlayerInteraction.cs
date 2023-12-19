using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interact Settings")]
    [SerializeField] private float maxDistance = 1f;
    private Camera cam;

    private IInteractable interactable;

    private void Awake()
    {
        cam = Camera.main;
    }

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
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, maxDistance) && raycastHit.collider != null)
        {
            GameObject detectedObject = raycastHit.collider.gameObject;
            if (detectedObject.TryGetComponent(out IInteractable newInteractable))
            {
                if (interactable != newInteractable)
                {
                    TryHideInteractableOutline();
                }

                interactable = newInteractable;
                interactable.Hover();
                return;
            }
        }
        TryHideInteractableOutline();
        interactable = null;
    }

    private void Interact()
    {
        if (interactable == null) return;
        interactable.Interact();
    }

    private void TryHideInteractableOutline()
    {
        if (interactable == null) return;
        interactable.NoHover();
    }
}
