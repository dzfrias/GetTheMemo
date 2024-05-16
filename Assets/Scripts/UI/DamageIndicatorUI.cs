using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicatorUI : MonoBehaviour
{
    [SerializeField] private Transform damageImagePivot;

    private Vector3 damageLocation;
    private Transform player;

    private float fadeOutTime = 1f;

    public void Setup(Vector3 damageLocation, Transform player)
    {
        this.damageLocation = damageLocation;
        this.player = player;
    }

    private void Start()
    {
        Destroy(gameObject, fadeOutTime);
    }

    private void Update()
    {
        damageLocation.y = player.transform.position.y;
        Vector3 direction = (damageLocation - player.position).normalized;
        float angle = Vector3.SignedAngle(direction, player.forward, Vector3.up);
        damageImagePivot.transform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
