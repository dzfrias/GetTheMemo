using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private float bobbingSpeed = 0.18f;
    [SerializeField] private float bobbingAmount = 0.2f;

    private float startY;
    private AudioSource footstep;

    private void Start()
    {
        startY = transform.localPosition.y;
        footstep = GetComponent<AudioSource>();
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

        var soundPos = Mathf.Sin(Time.time * bobbingSpeed * 1.5f);
        if (soundPos > 0.99 && footstep != null)
        {
            footstep.Play();
        }
        var pos = Mathf.Sin(Time.time * bobbingSpeed);
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            startY - pos * bobbingAmount,
            transform.localPosition.z
        );
    }
}
