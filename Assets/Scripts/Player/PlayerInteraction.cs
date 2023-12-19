using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interact Settings")]
    [SerializeField] private float maxDistance = 1f;
    private Camera cam;

    private IHoverable hoverable;

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
        OutlineDetection();
    }

    private void GameInput_OnInteract()
    {
        Interact();
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
