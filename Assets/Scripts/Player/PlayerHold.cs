using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHold : MonoBehaviour
{
    [SerializeField] private Transform holdAnchor;
    [SerializeField] private float holdDistance = 0.5f;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 holdPosition = ray.GetPoint(holdDistance);
        holdAnchor.position = holdPosition;
    }
}
