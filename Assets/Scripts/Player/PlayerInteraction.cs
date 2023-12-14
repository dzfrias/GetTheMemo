using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interact Box Settings")]
    [SerializeField] private Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
    [SerializeField] private float maxDistance = 1f;

    private void OnEnable()
    {
        GameInput.Instance.OnInteract += GameInput_OnInteract;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnInteract -= GameInput_OnInteract;
    }

    private void GameInput_OnInteract()
    {
        Interact();
    }

    private void Interact()
    {
        RaycastHit raycastHit;
        if (Physics.BoxCast(transform.position, halfExtents, transform.forward, out raycastHit, Quaternion.identity, maxDistance))
        {
            if (raycastHit.collider != null)
            {
                if (raycastHit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }
    }
}
