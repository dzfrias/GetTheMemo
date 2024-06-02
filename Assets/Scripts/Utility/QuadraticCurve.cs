using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticCurve : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float height;

    private Vector3 midPoint;

    public Vector3 Evaluate(float t)
    {
        Vector3 startToMid = Vector3.Lerp(startPoint.position, midPoint, t);
        Vector3 midToEnd = Vector3.Lerp(midPoint, endPoint.position, t);
        Vector3 curve = Vector3.Lerp(startToMid, midToEnd, t);
        return curve;
    }

    private void OnDrawGizmos()
    {
        if (startPoint == null || endPoint == null) return;

        SetMidpoint();

        for (int i = 0; i < 20; i++)
        {
            Gizmos.DrawWireSphere(Evaluate(i/20f), 0.1f);
        }
    }

    private void SetMidpoint()
    {
        Vector3 middle = Vector3.Lerp(startPoint.position, endPoint.position, 0.5f);
        midPoint = new Vector3(middle.x, height, middle.z);
    }
}
