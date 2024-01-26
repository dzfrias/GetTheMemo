using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundShockAttack : StateMachineBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private float expandSpeed;
    [SerializeField] private float startingRadius = 1f;
    [SerializeField] private float maxRadius = 15f;
    [SerializeField] private float timeUntilAttack = 0.5f;

    private Transform transform;

    private float radius;

    private List<(GameObject, float)> cubeInfos;

    private float timeElapsed;
    private bool isAttackStarted;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0f;
        isAttackStarted = false;
        transform = animator.transform;
        cubeInfos = new();
        radius = startingRadius;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > timeUntilAttack)
        {
            if (!isAttackStarted)
            {
                CreateCircle();
                isAttackStarted = true;
            }
            ExpandCircle();
        }
    }

    private void CreateCircle()
    {
        for (float radians = 0; radians < Mathf.PI * 2; radians += 0.05f)
        {
            float x = startingRadius * Mathf.Cos(radians);
            float y = 0f;
            float z = startingRadius * Mathf.Sin(radians);

            Vector3 spawnPosition = new Vector3(transform.position.x + x, y, transform.position.z + z);
            GameObject cube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
            cube.transform.position = spawnPosition;
            cubeInfos.Add((cube, radians));
        }
    }

    private void ExpandCircle()
    {
        radius += Time.deltaTime * expandSpeed;

        if (radius < maxRadius)
        {
            foreach ((GameObject, float) cubeInfo in cubeInfos)
            {
                float x = radius * Mathf.Cos(cubeInfo.Item2);
                float y = 0f;
                float z = radius * Mathf.Sin(cubeInfo.Item2);

                Vector3 newPosition = new Vector3(transform.position.x + x, y, transform.position.z + z);

                cubeInfo.Item1.transform.position = newPosition;
            }
        }
        else
        {
            foreach ((GameObject, float) cubeInfo in cubeInfos)
            {
                Destroy(cubeInfo.Item1);
            }
        }
    }
}
