using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interact Settings")]
    [SerializeField] private float maxDistance = 1f;
    [SerializeField] private Pointer pointer;

    [Header("Oultine Settings")]
    [SerializeField] private Color outlineColor;
    [SerializeField] private float outlineWidth = 5f;

    [Header("Pickup Settings")]
    [SerializeField] private Transform holdTransform;

    [Header("Click Settings")]
    [SerializeField] private float clickDistance;
    [SerializeField] private float clickStrength = 10f;

    private Camera cam;
    private GameObject hoverable;
    private IGrabbable heldObject;
    private int heldObjectLayer;
    private GameObject heldGameObject;
    private PlayerHold playerHold;
    private bool firstInteract = true;

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
        GameInput.Instance.OnClick += GameInput_OnClick;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnInteract -= GameInput_OnInteract;
        GameInput.Instance.OnPickup -= GameInput_OnPickup;
        GameInput.Instance.OnDrop -= GameInput_OnDrop;
        GameInput.Instance.OnClick -= GameInput_OnClick;
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
                heldObjectLayer = detectedObject.layer;
                heldGameObject = detectedObject;
                // We put it on the ignore raycast layer so the player can
                // still interact with things even when holding something
                detectedObject.layer = LayerMask.NameToLayer("Ignore Raycast");
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
            heldGameObject.layer = heldObjectLayer;
        }
    }

    private void GameInput_OnClick()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, clickDistance))
        {
            if (hit.rigidbody == null) return;
            hit.rigidbody.AddForceAtPosition(ray.direction * clickStrength, hit.point, ForceMode.Impulse);
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
            Outline outline = hoverable.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = outlineColor;
            outline.OutlineWidth = outlineWidth;
            pointer.OnHover();
            if (firstInteract)
            {
                OfficeManager.Instance.DisplayTutorial("E to interact");
            }
        }
        else
        {
            if (hoverable != null)
            {
                // Only called when an object is newly unhovered
                pointer.Reset();
                Destroy(hoverable.GetComponent<Outline>());
                if (firstInteract)
                {
                    OfficeManager.Instance.HideTutorial();
                }
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
                if (firstInteract)
                {
                    OfficeManager.Instance.HideTutorial();
                    firstInteract = false;
                }
            }
        }
    }

    private void TryNoHover()
    {
        if (hoverable == null) return;
        hoverable.layer = 0;
    }
}
