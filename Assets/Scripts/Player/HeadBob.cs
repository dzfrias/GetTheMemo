using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField]
    private float bobbingSpeed = 0.18f;

    [SerializeField]
    private float bobbingAmount = 0.2f;

    private float startY;

    private void Start()
    {
        startY = transform.localPosition.y;
    }

    private void Update()
    {
        Vector2 movement = GameInput.Instance.GetMovementVectorNormalized();
        if (movement == Vector2.zero)
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                new Vector3(transform.localPosition.x, startY, transform.localPosition.z),
                Time.deltaTime * bobbingSpeed
            );
            return;
        }

        transform.localPosition = new Vector3(
            transform.localPosition.x,
            startY - Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount,
            transform.localPosition.z
        );
    }
}
