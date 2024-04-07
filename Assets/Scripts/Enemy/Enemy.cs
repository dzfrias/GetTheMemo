using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected List<SkinnedMeshRenderer> skinnedMeshRenderers;
    [SerializeField] protected List<MeshRenderer> meshRenderers;
    [SerializeField] protected MMF_Player impactEffects;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected float attackDamage = 2f;
    [SerializeField] protected float attackDistance = 2f;
    [SerializeField] protected float attackCooldown = 3f;

    protected Transform player;
    protected NavMeshAgent navMeshAgent;
    protected Health health;
    protected StateMachine<EnemyState, StateEvent> enemyFSM;
    protected AnimationEventProxy animationEventProxy;

    protected float lastAttackTime;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        animator.TryGetComponent(out animationEventProxy);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void Start()
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
        animationEventProxy.OnAttack += DealDamage;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= Health_OnHealthChanged;
        animationEventProxy.OnAttack -= DealDamage;
    }
    
    private void Health_OnHealthChanged(float health)
    {
        enemyFSM.Trigger(StateEvent.Impact);
        impactEffects.PlayFeedbacks();
        StartCoroutine(Flash());
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Flash()
    {
        List<Color> savedMaterialColors = new();
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            foreach (Material material in skinnedMeshRenderer.materials)
            {
                savedMaterialColors.Add(material.color);
                material.color = Color.white;
            }
        }

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            foreach (Material material in meshRenderer.materials)
            {
                savedMaterialColors.Add(material.color);
                material.color = Color.white;
            }
        }

        yield return new WaitForSeconds(0.2f);
        int materialNumber = 0;
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            foreach (Material material in skinnedMeshRenderer.materials)
            {
                material.color = savedMaterialColors[materialNumber];
                materialNumber++;
            }
        }

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            foreach (Material material in meshRenderer.materials)
            {
                material.color = savedMaterialColors[materialNumber];
                materialNumber++;
            }
        }
    }

    public virtual void OnAttack(State<EnemyState, StateEvent> _)
    {
        transform.LookAt(player.transform.position);
        lastAttackTime = Time.time;
    }

    private void DealDamage()
    {
        RaycastHit[] raycastHits = Physics.RaycastAll(attackPoint.position, transform.forward, attackDistance);
        foreach (RaycastHit raycastHit in raycastHits)
        {
            if (raycastHit.collider.CompareTag("Player"))
            {
                player.GetComponent<Health>().TakeDamage(attackDamage);
            }
        }
    }

    public Animator GetAnimator() 
    {
        return animator;
    }
}
