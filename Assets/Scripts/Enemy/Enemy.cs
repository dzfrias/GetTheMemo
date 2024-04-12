using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemySO enemySO;

    [SerializeField] protected List<Renderer> renderers;
    [SerializeField] protected MMF_Player impactEffects;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform attackPoint;

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
        StartCoroutine(Dissolve(-1f));
    }

    public virtual void AddStatesToEnemyFSM()
    {
        enemyFSM.AddState(EnemyState.Idle, new IdleState(false, this));
        enemyFSM.AddState(EnemyState.Chase, new ChaseState(false, this, player));

        enemyFSM.AddState(EnemyState.Impact, new ImpactState(true, this, exitTime: 1.5f));
        enemyFSM.AddState(EnemyState.Death, new DeathState(false, this));

        enemyFSM.SetStartState(EnemyState.Chase);
    }

    public virtual void AddEnemyStateTransitions()
    {
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase, ShouldChase));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Impact, EnemyState.Chase, ShouldChase));

        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, IsWithinIdleRange));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Impact, EnemyState.Idle, IsWithinIdleRange));

        enemyFSM.AddTriggerTransitionFromAny(StateEvent.Impact, EnemyState.Impact, forceInstantly: true);
        enemyFSM.AddTriggerTransitionFromAny(StateEvent.Death, EnemyState.Death, forceInstantly: true);
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
        
        if (animationEventProxy != null) animationEventProxy.OnAttack += DealDamage;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= Health_OnHealthChanged;
        if (animationEventProxy != null) animationEventProxy.OnAttack -= DealDamage;
    }
    
    private void Health_OnHealthChanged(float health)
    {
        enemyFSM.Trigger(StateEvent.Impact);
        impactEffects.PlayFeedbacks();
        StartCoroutine(Flash());
        if (health <= 0)
        {
            enemyFSM.Trigger(StateEvent.Death);
            StartCoroutine(Die());
        }
    }

    private IEnumerator Flash()
    {
        var saved = new List<Color>();
        foreach (var renderer in renderers)
        {
            saved.Add(renderer.material.color);
            renderer.material.color = Color.white;
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material.color = saved[i];
        }
    }

    private IEnumerator Dissolve(float speed = 1f)
    {
        var saved = new List<Material>();
        foreach (Renderer renderer in renderers)
        {
            var old = renderer.material;
            saved.Add(old);
            var newMat = new Material(Shader.Find("Shader Graphs/Dissolve"));
            CopyMaterialFloat("_Smoothness", old, newMat);
            CopyMaterialFloat("_Metallic", old, newMat);
            newMat.SetFloat("_Speed", speed);
            newMat.SetFloat("_Start", Time.time);
            newMat.color = old.color;
            renderer.material = newMat;
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material = saved[i];
        }
    }

    private IEnumerator Die()
    {
        yield return StartCoroutine(Dissolve(0.5f));
        Destroy(gameObject);
    }

    private void CopyMaterialFloat(string name, Material src, Material dst)
    {
        if (src.HasFloat(name))
        {
            dst.SetFloat(name, src.GetFloat(name));
        }
    }

    public virtual void OnAttack(State<EnemyState, StateEvent> _)
    {
        transform.LookAt(player.transform.position);
        lastAttackTime = Time.time;
    }

    private void DealDamage()
    {
        RaycastHit[] raycastHits = Physics.RaycastAll(attackPoint.position, transform.forward, enemySO.attackDistance);
        foreach (RaycastHit raycastHit in raycastHits)
        {
            if (raycastHit.collider.CompareTag("Player"))
            {
                player.GetComponent<Health>().TakeDamage(enemySO.attackDamage);
            }
        }
    }

    public Animator GetAnimator() 
    {
        return animator;
    }
}
