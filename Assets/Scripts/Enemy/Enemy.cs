using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected float attackCooldown = 3f;

    protected Transform player;
    protected NavMeshAgent navMeshAgent;
    protected Health health;
    protected StateMachine<EnemyState, StateEvent> enemyFSM;

    protected float lastAttackTime;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        enemyFSM = new StateMachine<EnemyState, StateEvent>();
        AddStatesToEnemyFSM();
        AddEnemyStateTransitions();
        enemyFSM.Init();
    }

    public virtual void AddStatesToEnemyFSM()
    {
        enemyFSM.AddState(EnemyState.Idle, new IdleState(false, this));
        enemyFSM.AddState(EnemyState.Chase, new ChaseState(false, this, player));

        enemyFSM.AddState(EnemyState.Impact, new ImpactState(true, this, exitTime: 1.5f));

        enemyFSM.SetStartState(EnemyState.Chase);
    }

    public virtual void AddEnemyStateTransitions()
    {
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase, ShouldChase));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Impact, EnemyState.Chase, ShouldChase));

        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, IsWithinIdleRange));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Impact, EnemyState.Idle, IsWithinIdleRange));

        enemyFSM.AddTriggerTransitionFromAny(StateEvent.Impact, EnemyState.Impact, forceInstantly: true);
    }

    private bool IsWithinIdleRange(Transition<EnemyState> _)
    {
        return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    }

    private bool ShouldChase(Transition<EnemyState> transition)
    {
        return Vector3.Distance(player.transform.position, transform.position) > navMeshAgent.stoppingDistance;
    }

    private void Update()
    {
        enemyFSM.OnLogic();
    }

    private void OnEnable()
    {
        health.OnHealthChanged += Health_OnHealthChanged;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= Health_OnHealthChanged;
    }
    
    private void Health_OnHealthChanged(float health)
    {
        enemyFSM.Trigger(StateEvent.Impact);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnAttack(State<EnemyState, StateEvent> _)
    {
        transform.LookAt(player.transform.position);
        lastAttackTime = Time.time;
    }

    public Animator GetAnimator() 
    {
        return animator;
    }
}
