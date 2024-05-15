using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    [SerializeField] private float headSlideForce = 0.025f;

    private void OnTriggerStay(Collider collider)
    {
        if (!collider.CompareTag("Player")) return;

        GameObject player = collider.gameObject;

        player.GetComponent<CharacterController>().Move(player.transform.forward * headSlideForce);
    }
}
