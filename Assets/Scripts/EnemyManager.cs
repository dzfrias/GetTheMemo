using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [SerializeField] private float radius = 1.5f;
    [SerializeField] private int pointsAroundPlayer = 8;

    private Transform player;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There is already an Enemy Manager in the scene");
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public Vector3 GetDestinationAroundPlayer(int index)
    {
        Vector3 destination = new Vector3(
            player.position.x + radius * Mathf.Cos(2 * Mathf.PI * index / pointsAroundPlayer),
            player.position.y,
            player.position.z + radius * Mathf.Sin(2 * Mathf.PI * index / pointsAroundPlayer)
        );
        return destination;
    }

    public int GetRandomDestinationIndex()
    {
        int randomIndex = Random.Range(0, pointsAroundPlayer);
        return randomIndex;
    }

    public float GetRadius()
    {
        return radius;
    }
}
