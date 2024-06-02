using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class BossButton : MonoBehaviour, IInteractable
{
    private MMF_Player player;
    private Collider collider;

    private void Start()
    {
        player = GetComponent<MMF_Player>();
        collider = GetComponent<Collider>();
    }

    public void Interact(Vector3 playerPos)
    {
        OfficeManager.Instance.NextBeat();
        player.PlayFeedbacks();
        collider.enabled = false;
    }
}
