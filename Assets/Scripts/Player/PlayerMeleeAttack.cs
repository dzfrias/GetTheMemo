using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform head;
    [SerializeField] private float attackDistance = 5f;
    [SerializeField] private float attackDelay = 0.5f;
    private bool isAttacking = false;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameInput.Instance.OnAttack += Attack;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnAttack -= Attack;
    }

    private void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger("Attack");
        Invoke(nameof(HitObject), attackDelay);
    }

    private void HitObject()
    {
        Vector3 middleOfScreen = new Vector3(0.5f, 0.5f, 0f);
        Ray ray = cam.ViewportPointToRay(middleOfScreen);
        Debug.DrawRay(ray.GetPoint(0), ray.direction, Color.red, 5f);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Debug.Log("Hit: " + raycastHit.collider.gameObject.name);
        }
        isAttacking = false;
    }
}
