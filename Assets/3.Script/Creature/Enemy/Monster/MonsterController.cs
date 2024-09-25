using System;
using System.Collections.Generic;
using System.Xml;
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
    [SerializeField] public float PatrolStopDistance = 0.5f;
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
    [field:SerializeField] public bool isCanPatrol { get; private set; }
    public bool isFoundPlayer = false;
    //Patrol Node
    [field : SerializeField] public List<Transform> nodeList { get; private set; }

    public Stack<Transform> nodeStack { get; private set; }

    //etc...
    [SerializeField] private float _rotLerp = 0.03f;
    public Timer attackTimer { get; private set; }
    private void Start()
    {
        //GetComponent
        navAI = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        aniDataBase = new MonsterAniDataBase();
        aniDataBase.init();

        //Inits
        nodeStack = new Stack<Transform>();
        attackTimer = new Timer(2.0f, this);
        navAI.updateRotation = false;

        //Init States
        stateMachine = new MonsterStateMachine(this);
        var _locoState = new MonsterLocoState(stateMachine);
        var _battleState = new MonsterBattleState(stateMachine);
        stateMachine.AddTransition(_locoState, _battleState, new FuncPredicate(() => isFoundPlayer));
        stateMachine.AddTransition(_battleState, _locoState, new FuncPredicate(() => !isFoundPlayer));
        stateMachine.SetState(_locoState);
    }

    private void Update()
    {
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereRad);
    }
}
