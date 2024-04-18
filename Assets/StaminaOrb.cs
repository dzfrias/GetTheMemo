using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowPlayer))]
public class StaminaOrb : MonoBehaviour
{
    public float regenerateAmount = 5f;

    private FollowPlayer follow;

    private void Awake()
    {
        follow = GetComponent<FollowPlayer>();
    }

    private void OnEnable()
    {
        follow.OnHit += AddStamina;
    }

    private void OnDisable()
    {
        follow.OnHit -= AddStamina;
    }

    private void AddStamina(Collider player)
    {
        var pm = player.gameObject.GetComponent<PlayerMovement>();
        pm.RegenerateStamina(regenerateAmount);
    }
}
