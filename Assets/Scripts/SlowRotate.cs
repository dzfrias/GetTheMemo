using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotate : MonoBehaviour
{
    [SerializeField] private Vector2 rotationRange = new Vector2(10f, 20f);
    [SerializeField] private Vector2 pivotRange = new Vector2(3f, 5f);

    private float rotationSpeedX;
    private float rotationSpeedY;
    private float rotationSpeedZ;

    private void Start()
    {
        StartCoroutine(ChangeRotation());
    }

    private void Update()
    {
        transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime);
    }

    private IEnumerator ChangeRotation()
    {
        while (true)
        {
            rotationSpeedX = GenerateRandom();
            rotationSpeedY = GenerateRandom();
            rotationSpeedZ = GenerateRandom();
            yield return new WaitForSeconds(Random.Range(pivotRange.x, pivotRange.y));
        }
    }

    private float GenerateRandom()
    {
        float value = rotationSpeedX = Random.Range(rotationRange.x, rotationRange.y);
        if (Random.value > 0.5)
        {
            value *= -1;
        }
        return value;
    }
}
