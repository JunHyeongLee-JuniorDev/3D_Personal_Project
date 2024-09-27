using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterController : MonoBehaviour
{
    //UnityComponents
    public NavMeshAgent navAI {  get; private set; }
    public Animator animator {  get; private set; }

    //Monster Data
    [field : SerializeField] public MonsterSO monsterSO { get; private set; }
    [field : SerializeField] public float PatrolStopDistance { get; private set; } = 0.5f;
    [field : SerializeField] public float ChaseStopDistance { get; private set; } = 3.0f;
    public MonsterAniDataBase aniDataBase { get; private set; }

    //State Machine
    private MonsterStateMachine stateMachine;

    [Header("Monster View")]
    //Monster View
    [SerializeField] private float sphereRad;
    [SerializeField] private float viewDegree;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacles;

    //Player
    public Transform player { get; private set; }

    //State Conditions
    [HideInInspector] public bool isFoundPlayer = false;
    [HideInInspector] public bool isAttack = false;
    [HideInInspector] public bool isInRotateRad = false;

    //Patrol Node
    [field : SerializeField] public List<Transform> nodeList { get; private set; }

    public Stack<Transform> nodeStack { get; private set; }

    //etc...
    [field: SerializeField] public float _rotLerp { get; private set; } = 0.03f;


    //Timer
    [SerializeField] private float timeBetweenAttack = 1.0f;
    public Timer attackTimer { get; private set; }
    public Timer outMapTimer { get; private set; }

    private void Start()
    {
        //GetComponent
        navAI = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        aniDataBase = new MonsterAniDataBase();
        aniDataBase.init();

        //Inits
        nodeStack = new Stack<Transform>();
        attackTimer = new Timer(timeBetweenAttack, this);
        outMapTimer = new Timer(5.0f, this);
        navAI.updateRotation = false;

        //Init States
        stateMachine = new MonsterStateMachine(this);

        var _locoState = new MonsterLocoState(stateMachine);
        var _chaseState = new MonsterChaseState(stateMachine);
        var _rotateState = new MonsterRotateState(stateMachine);
        var _attackState = new MonsterAttackState(stateMachine);

        stateMachine.AddAnyTransition(_locoState, new FuncPredicate(() => !isFoundPlayer));
        stateMachine.AddAnyTransition(_chaseState, new FuncPredicate(() => isFoundPlayer && !isInRotateRad && !isAttack));
        stateMachine.AddAnyTransition(_rotateState, new FuncPredicate(() => isInRotateRad && !isAttack));
        stateMachine.AddAnyTransition(_attackState, new FuncPredicate(() => isAttack));
        stateMachine.AddTransition(_attackState, _rotateState, new FuncPredicate(() => !isAttack));

        stateMachine.SetState(_locoState);
    }

    private void Update()
    {
        PlayerOutOfMapCheck();
        stateMachine.Update();
        IsPlayerInView();
        transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, navAI.desiredVelocity, _rotLerp));
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
            Debug.Log("플레이어 나간거 체크 되니???");
            TurnOffNav();
            isFoundPlayer = false;
            animator.CrossFade(aniDataBase.monsterAniClips[EMonsterAni.Idle], 0.2f);

            outMapTimer.StartTimer(() =>
            {
                Debug.Log("아마 타이머가 바로 끝나는게 아닐까?");
                TurnOnNav();
            });
        }

        else if(outMapTimer.isTickin && 
            navAI.pathStatus == NavMeshPathStatus.PathComplete &&
            !navAI.pathPending &&
            isFoundPlayer)
        {
            navAI.isStopped = false;
            outMapTimer.StopTimer();
        }
    }

    public void TurnOnNav()
    {
        navAI.ResetPath();
        navAI.isStopped = false;
        navAI.updatePosition = true;
    }

    public void TurnOffNav()
    {
        navAI.isStopped = true;
        navAI.updatePosition = false;
        navAI.velocity = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRad);
    }
}
