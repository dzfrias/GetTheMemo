using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> throwableObjectPrefabs;


    [SerializeField]
    private float spawnRate = 5f;

    [SerializeField]
    private float radius;

    private float spawnTimer;
    private bool isValidSpawnLocation = false;
    private GameObject spawnCheck;

    private Transform playerTransform;


    private void Awake()
    {
        spawnTimer = spawnRate;
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (spawnTimer <= 0)
        {
            GameObject randomThrowableObjectPrefab = GetRandomThrowableObjectPrefab();
            Vector3 spawnLocation = GetValidSpawnLocation(randomThrowableObjectPrefab);
            Instantiate(randomThrowableObjectPrefab, spawnLocation, Quaternion.identity);
            spawnTimer = spawnRate;
        }
        spawnTimer -= Time.deltaTime;
    }

    private Vector3 GetValidSpawnLocation(GameObject throwableObjectPrefab)
    {
        isValidSpawnLocation = false;
        Vector3 position = Vector3.zero;
        while (!isValidSpawnLocation)
        {
            position = GetRandomLocation();
            Vector3 halfExtents = GetColliderSize(throwableObjectPrefab)/2;
            Collider[] hitColliders = Physics.OverlapBox(position, halfExtents, Quaternion.identity);
            if (IsValidSpawnLocation(hitColliders))
            {
                isValidSpawnLocation = true;
            }
        }
        Debug.Log("Found Valid Location: " + position);
        return position;
    }

    private bool IsValidSpawnLocation(Collider[] hitColliders)
    {
        if (hitColliders.Length > 0)
        {
            return false;
        }
        return true;
    }

    private Vector3 GetColliderSize(GameObject throwableObjectPrefab)
    {
        Collider collider = throwableObjectPrefab.GetComponent<Collider>();
        return collider.bounds.size;
    }

    private GameObject GetRandomThrowableObjectPrefab()
    {
        int randomIndex = Random.Range(0, throwableObjectPrefabs.Count);
        return throwableObjectPrefabs[randomIndex];
    }

    private Vector3 GetRandomLocation()
    {
        Vector3 playerPosition = playerTransform.position;
        float randomX = Random.Range(playerPosition.x - radius, playerPosition.x + radius);
        float randomZ = Random.Range(playerPosition.z - radius, playerPosition.z + radius);
        Vector3 location = new Vector3(randomX, 2f, randomZ);
        return location;
    }
}
