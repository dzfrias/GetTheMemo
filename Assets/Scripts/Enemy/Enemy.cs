using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

public class Enemy : MonoBehaviour
{
    public static event System.Action OnEnemyDeath;

    [SerializeField] protected EnemySO enemySO;

    [SerializeField] protected List<Renderer> renderers;
    [SerializeField] protected MMF_Player impactEffects;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Transform attackPoint;

    [Header("Death Spawn")]
    [SerializeField] protected GameObject toSpawn;
    [SerializeField] [Range(0f, 1f)] protected float probability = 0.5f;

    [Header("Stimulation Stats")]
    [SerializeField] protected float stimulatedSpeedMultiplier = 1.5f;
    [SerializeField] protected float stimulatedAttackCooldownDivider = 0.5f;
    [SerializeField] protected float stimulatedSizeMultiplier = 1.25f;

    protected Transform player;
    protected NavMeshAgent navMeshAgent;
    protected Health health;
    protected StateMachine<EnemyState, StateEvent> enemyFSM;
    protected AnimationEventProxy animationEventProxy;

    protected float lastAttackTime;
    protected float attackCooldown;
    protected bool isStimulated;

    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        animator.TryGetComponent(out animationEventProxy);

        attackCooldown = enemySO.attackCooldown;

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

        enemyFSM.AddState(EnemyState.Death, new DeathState(false, this));

        enemyFSM.SetStartState(EnemyState.Chase);
    }

    public virtual void AddEnemyStateTransitions()
    {
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle, EnemyState.Chase, ShouldChase));

        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Idle, IsWithinIdleRange));
        enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack, EnemyState.Chase, ShouldChase));

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

    protected virtual void Update()
    {
        enemyFSM.OnLogic();
    }

    private void OnEnable()
    {
        health.OnHealthChanged += Health_OnHealthChanged;
        
        if (animationEventProxy != null) animationEventProxy.OnPrimaryAttack += PrimaryAttack;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= Health_OnHealthChanged;
        if (animationEventProxy != null) animationEventProxy.OnPrimaryAttack -= PrimaryAttack;
    }
    
    private void Health_OnHealthChanged(float health)
    {
        enemyFSM.Trigger(StateEvent.Impact);
        impactEffects?.PlayFeedbacks();
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
            Vector4 oldColor = old.color;
            // This will clamp the vector's magnitude to 10
            float magnitude = Mathf.Min(oldColor.magnitude, 10f);
            newMat.color = oldColor.normalized * magnitude;
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
        OnEnemyDeath?.Invoke();
        yield return StartCoroutine(Dissolve(0.5f));
        if (toSpawn != null && Random.value > probability)
        {
            Instantiate(toSpawn, transform.position, Quaternion.identity);
        }
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

    protected virtual void PrimaryAttack()
    {
        Collider[] hitColliders = Physics.OverlapBox(attackPoint.position, enemySO.attackBox/2, transform.rotation);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                player.GetComponent<Health>().TakeDamage(enemySO.attackDamage);
            }
        }
    }

    public IEnumerator Stimulate(float stimulateTime)
    {
        isStimulated = true;

        float normalSpeed = navMeshAgent.speed;
        float normalAttackCooldown = enemySO.attackCooldown;
        Vector3 normalScale = gameObject.transform.localScale;

        navMeshAgent.speed *= stimulatedSpeedMultiplier;
        attackCooldown /= stimulatedAttackCooldownDivider;
        transform.localScale *= stimulatedSizeMultiplier;

        yield return new WaitForSeconds(stimulateTime);

        navMeshAgent.speed = normalSpeed;
        attackCooldown = normalAttackCooldown;
        transform.localScale = normalScale;

        isStimulated = false;
    }

    public bool IsStimulated()
    {
        return isStimulated;
    }

    public Animator GetAnimator() 
    {
        return animator;
    }
}
