using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSpawn : MonoBehaviour
{
    private void Awake()
    {
        if (SaveData.Instance.data.currentLevel == 0) return;
        Transform player = GameObject.FindWithTag("Player").transform;
        player.position = transform.position;
        player.rotation = transform.rotation;
    }
}
