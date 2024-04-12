using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class RangedEnemy : Enemy
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shotDelay = 0.1f;

    private float projectileSpeed;
    private CharacterController playerController;

    public override void Start()
    {
        base.Start();
        playerController = player.gameObject.GetComponent<CharacterController>();
        projectileSpeed = projectile.GetComponent<Projectile>().speed;
    }

    public override void AddStatesToEnemyFSM()
    {
        base.AddStatesToEnemyFSM();
        enemyFSM.AddState(EnemyState.Attack, new AttackState(true, this, OnAttack, exitTime: 4f));
    }

    public override void OnAttack(State<EnemyState, StateEvent> _)
    {
        base.OnAttack(_);
        StartCoroutine(Shoot());
    }

    public override void AddEnemyStateTransitions()
    {
        base.AddEnemyStateTransitions();
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack, ShouldShoot));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Attack, ShouldShoot));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Impact, EnemyState.Attack, ShouldShoot));
    }

    private bool ShouldShoot(Transition<EnemyState> _)
    {
        return lastAttackTime + enemySO.attackCooldown < Time.time && IsInShootingRange();
    }

    private bool IsInShootingRange()
    {
        return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    }

    private IEnumerator Shoot()
    {
        Instantiate(projectile, transform.position, Quaternion.LookRotation(transform.forward));
        yield return new WaitForSeconds(shotDelay);
        Instantiate(projectile, transform.position, PredictRotation(projectileSpeed));
        yield return new WaitForSeconds(shotDelay);
        Instantiate(projectile, transform.position, PredictRotation(projectileSpeed * 2));
    }

    private Quaternion PredictRotation(float speed)
    {
        var expectedTime = Vector3.Distance(transform.position, player.position) / speed;
        var predictedLocation = player.position + playerController.velocity * expectedTime;
        var predicted = Quaternion.LookRotation(predictedLocation - transform.position);
        return predicted;
    }
}
