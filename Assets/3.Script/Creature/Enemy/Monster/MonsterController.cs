using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterController : MonoBehaviour
{
    //UnityComponents
    public NavMeshAgent navAI {  get; protected set; }
    public Animator animator {  get; protected set; }
    public MonsterWeaponTrigger weaponTrigger { get; protected set; }
    protected ParticleSystem bloodEFF;
    [SerializeField] protected Collider hitBox;

    //Monster Data
    [field : SerializeField] public MonsterSO monsterSO { get; protected set; }
    [field : SerializeField] public monsterSaveData statData { get; protected set; }
    [field : SerializeField] public float PatrolStopDistance { get; protected set; } = 0.5f;
    [field : SerializeField] public float ChaseStopDistance { get; protected set; } = 3.0f;

    public MonsterAniDataBase aniDataBase { get; protected set; }

    //Events
    public UnityAction OnSkill;
    public UnityAction<Transform> OnPlayerFound;

    //State Machine
    public MonsterStateMachine stateMachine { get; protected set; }

    [Header("Monster View")]
    //Monster View
    [SerializeField] protected float sphereRad;
    [SerializeField] protected float viewDegree;
    [SerializeField] protected LayerMask targetMask;
    [SerializeField] protected LayerMask obstacles;

    //Player
    public Transform player;

    //State Conditions
    [HideInInspector] public bool isFoundPlayer = false;
    [HideInInspector] public bool isAttack = false;
    [HideInInspector] public bool isInRotateRad = false;
    [HideInInspector] public bool isDead = false;

    //Patrol Node
    [field : SerializeField] public List<Transform> nodeList { get; private set; }

    public Stack<Transform> nodeStack { get; private set; }

    //etc...
    [field: SerializeField] public float _rotLerp { get; protected set; } = 0.03f;

    //UI
    [SerializeField] protected MonsterSlider hpPrefabs;
    public MonsterSlider hpSlider { get; protected set; }

    //Timer
    public Timer attackTimer { get; protected set; }
    public Timer outMapTimer { get; protected set; }
    public Timer hurtTimer { get; protected set; }
    public Timer deathTimer { get; protected set; }

    protected virtual void Start()
    {
        //GetComponent
        navAI = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        weaponTrigger = GetComponentInChildren<MonsterWeaponTrigger>();
        bloodEFF = GetComponentInChildren<ParticleSystem>();

        InitStatData();
        statData = Managers.Instance.Data.LoadMonsterData(statData.monsterID, statData);
        Managers.Instance.Game.playerController.OnPlayerDead -= OnPlayerDead;
        Managers.Instance.Game.playerController.OnPlayerDead += OnPlayerDead;
        Managers.Instance.Game.monsters.Add(this);
        aniDataBase = Managers.Instance.Data.monsterAniDB;

        //Inits
        nodeStack = new Stack<Transform>();
        attackTimer = new Timer(0.0f, this);
        outMapTimer = new Timer(5.0f, this);
        hurtTimer = new Timer(1.0f, this);
        deathTimer = new Timer(3.0f, this);
        hpSlider = Instantiate(hpPrefabs);
        SethpSliderPos();
        RefillHealth();
        hpSlider.gameObject.SetActive(false);
        navAI.updateRotation = false;

        //Init States
        stateMachine = new MonsterStateMachine(this);

        var _locoState = new MonsterLocoState(stateMachine);
        var _chaseState = new MonsterChaseState(stateMachine);
        var _rotateState = new MonsterRotateState(stateMachine);
        var _attackState = new MonsterAttackState(stateMachine);
        var _deathState = new MonsterDeathState(stateMachine);

        stateMachine.AddAnyTransition(_locoState, new FuncPredicate(() => !isFoundPlayer && !isDead));
        stateMachine.AddAnyTransition(_chaseState, new FuncPredicate(() => isFoundPlayer && !isInRotateRad && !isAttack && !isDead));
        stateMachine.AddAnyTransition(_rotateState, new FuncPredicate(() => isInRotateRad && !isAttack && !isDead));
        stateMachine.AddAnyTransition(_attackState, new FuncPredicate(() => isAttack && !hurtTimer.isTickin && !isDead));
        stateMachine.AddAnyTransition(_deathState, new FuncPredicate(() => isDead));
        stateMachine.AddTransition(_attackState, _rotateState, new FuncPredicate(() => !isAttack));

        stateMachine.SetState(_locoState);
    }

    protected virtual void Update()
    {
        PlayerOutOfMapCheck();
        stateMachine.Update();
        IsPlayerInView();

        if (navAI.desiredVelocity != Vector3.zero)
        {
            Quaternion direction = Quaternion.LookRotation(navAI.desiredVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, _rotLerp * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    private void LateUpdate()
    {
        stateMachine.LateUpdate();
    }

    /// <summary>
    /// 구 내부 시야각 안에서 플레이어 감지
    /// </summary>
    private void IsPlayerInView()
    {
        if (isFoundPlayer) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereRad, layerMask : targetMask);


        Vector3 _rightDir = ConvertDegreeToDir(transform.eulerAngles.y + viewDegree * 0.5f);
        Vector3 _leftDir = ConvertDegreeToDir(transform.eulerAngles.y - viewDegree * 0.5f);

        Debug.DrawRay(transform.position, _rightDir * sphereRad, Color.green);
        Debug.DrawRay(transform.position, _leftDir * sphereRad, Color.green);
        
        if (colliders.Length == 0) return;

        foreach (var _target in colliders)
        {
            /*
             * 타겟의 위치가 각도 내에 있는지 확인하기
             */
            
            Vector3 dirTarget = _target.transform.position - transform.position;

            if (Vector3.Angle(transform.forward, dirTarget.normalized) < viewDegree * 0.5f)
            {
                if (!Physics.Raycast(transform.position, dirTarget.normalized, 
                    Vector3.Distance(transform.position, _target.transform.position),obstacles))
                {
                    player = _target.transform;
                    OnPlayerFound?.Invoke(player);
                    isFoundPlayer = true;
                    return;
                }
            }
        }
        isFoundPlayer = false;
    }

    private Vector3 ConvertDegreeToDir(float degree)
    {
        float _rad = Mathf.Deg2Rad * degree;
        return new Vector3(Mathf.Sin(_rad),0.0f, Mathf.Cos(_rad));
    }

    public bool CheckPlayerDistance()
    {
        return !navAI.pathPending && (navAI.remainingDistance <= navAI.stoppingDistance);
    }

    private void PlayerOutOfMapCheck()
    {
        if (!navAI.pathPending && navAI.pathStatus == NavMeshPathStatus.PathPartial &&
            !outMapTimer.isTickin)
        {
            Debug.Log("일단 여기 들어오는지 확인");
            TurnOffNav();
            isFoundPlayer = false;
            isInRotateRad = false;
            isAttack = false;
            animator.CrossFade(aniDataBase.monsterAniClips[EMonsterAni.Idle], 0.2f);
            animator.SetFloat(aniDataBase.monsterParams[EMonsterAni.LocoBlend], 0.0f);
            outMapTimer.StartTimer(() =>
            {
                TurnOnNav();
            });
        }
    }

    protected virtual void InitStatData()
    {
        statData.defaultSpawnPos = transform.position;
        statData.currentHealth = monsterSO.MaxHealth;
        statData.maxHealth = monsterSO.MaxHealth;
        statData.isDead = false;
        statData.isBoss = false;
    }

    protected virtual void SethpSliderPos()
    {
        hpSlider.transform.SetParent(transform);
        hpSlider.transform.localPosition = Vector3.zero;
        Vector3 UIoffSet = hpSlider.transform.localPosition;
        UIoffSet.y = hitBox.bounds.size.y * 1.1f;
        hpSlider.transform.localPosition = UIoffSet;
    }

    public void TurnOnNav()
    {
        if (navAI.isActiveAndEnabled && gameObject.activeSelf)
        {
            navAI.enabled = true;
            navAI.ResetPath();
            navAI.isStopped = false;
            navAI.updatePosition = true;
        }
    }

    public void TurnOffNav()
    {
        if (navAI.isActiveAndEnabled && gameObject.activeSelf)
        {
            navAI.isStopped = true;
            navAI.updatePosition = false;
            navAI.velocity = Vector3.zero;
        }
    }

    public void OnPlayerDead()
    {
        TurnOffNav();
        isFoundPlayer = false;
        isInRotateRad = false;
        isAttack = false;
        player = null;
    }

    public void activefalseForDeath()
    {
        deathTimer.StartTimer(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void ReduceHealth(float damage)
    {
        statData.currentHealth -= damage;
        stateMachine?.OnHurt();
        hpSlider.ChangeSliderWithFake(statData.currentHealth, statData.maxHealth);
    }

    public void RefillHealth()
    {
        statData.currentHealth = statData.maxHealth;
    }

    public void ShowHealthUI()
    {
        hpSlider.gameObject.SetActive(true);
    }

    public void OnReset()
    {
        /*
         * 피를 채워야하고
         * 포지션도 설정한 포지션으로 저장하고
         * bool 값 다 true로 하고
         */

        RefillHealth();
        statData.isDead = false;
        isAttack = false;
        isFoundPlayer = false;
        isInRotateRad = false;
        player = null;
    }

    public void SprayBlood()
    {
        Vector3 targetPos = Managers.Instance.Game.playerController.transform.position;
        targetPos.y = transform.position.y;

        bloodEFF.transform.LookAt(targetPos);
        bloodEFF.Play();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRad);
    }
}
