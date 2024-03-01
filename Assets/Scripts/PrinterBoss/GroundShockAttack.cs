using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundShockAttack : StateMachineBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private float expandSpeed;
    [SerializeField] private float startingRadius = 1f;
    [SerializeField] private float maxRadius = 15f;
    [SerializeField] private float timeUntilAttack = 0.5f;

    private Transform transform;

    private float radius;

    private List<HitboxInfo> hitboxInfos;

    private struct HitboxInfo
    {
        public HitboxInfo(GameObject gameObject, float radians)
        {
            this.gameObject = gameObject;
            this.radians = radians;
        }

        public GameObject gameObject;
        public float radians;
    }

    private float timeElapsed;
    private bool isAttackStarted;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0f;
        isAttackStarted = false;
        transform = animator.transform;
        hitboxInfos = new();
        radius = startingRadius;

        animator.applyRootMotion = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        DestroyHitboxes();

        animator.applyRootMotion = false;
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
            GameObject shockHitboxCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
            GroundShockHitbox groundShockHitbox = shockHitboxCube.GetComponent<GroundShockHitbox>();
            groundShockHitbox.OnHit += ShockHitboxCube_OnHit;
            shockHitboxCube.transform.position = spawnPosition;
            hitboxInfos.Add(new HitboxInfo(shockHitboxCube, radians));
        }
    }

    private void ExpandCircle()
    {
        radius += Time.deltaTime * expandSpeed;

        if (radius < maxRadius)
        {
            foreach (HitboxInfo hitboxInfo in hitboxInfos)
            {
                float x = radius * Mathf.Cos(hitboxInfo.radians);
                float y = 0f;
                float z = radius * Mathf.Sin(hitboxInfo.radians);

                Vector3 newPosition = new Vector3(transform.position.x + x, y, transform.position.z + z);

                hitboxInfo.gameObject.transform.position = newPosition;
            }
        }
        else
        {
            DestroyHitboxes();
        }
    }

    private void ShockHitboxCube_OnHit(Health health)
    {
        DestroyHitboxes();
        health.TakeDamage(damage);
    }

    private void DestroyHitboxes()
    {
        foreach (HitboxInfo hitboxInfo in hitboxInfos)
        {
            GameObject hitboxObject = hitboxInfo.gameObject;

            GroundShockHitbox groundShockHitbox = hitboxObject.GetComponent<GroundShockHitbox>();
            groundShockHitbox.OnHit -= ShockHitboxCube_OnHit;

            Destroy(hitboxObject);
        }
        hitboxInfos.Clear();
    }
}
