using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : MonoBehaviour
{
    [SerializeField] private float radius = 5f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float speedMultiplier = 1.1f;
    [SerializeField] private Vector2 pauseRange = new Vector2(1f, 2f);

    private Vector3 startPos;
    private Vector3 target;
    private float currentSpeed;

    private void Start()
    {
        startPos = transform.position;
        target = transform.position;
        StartCoroutine(Move());
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);
        currentSpeed *= speedMultiplier;
    }

    private IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitWhile(() => Vector3.Distance(transform.position, target) >= 0.001f);
            float pauseTime = Random.Range(pauseRange.x, pauseRange.y);
            yield return new WaitForSeconds(pauseTime);
            target = startPos + Random.insideUnitSphere * radius;
            currentSpeed = speed;
        }
    }
}
