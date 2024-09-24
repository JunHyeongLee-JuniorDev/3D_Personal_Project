using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterController : MonoBehaviour
{
    //NavMeshAgent
    private NavMeshAgent navAI;

    //State Machine
    private MonsterStateMachine stateMachine;

    [Header("Monster View")]
    //Monster View
    [SerializeField] private float sphereRad;
    [SerializeField] private float viewDegree;
    [SerializeField] private LayerMask targetMask;

    //Player
    public Transform player;

    //State Conditions
    public bool isFoundPlayer;

    //Patrol Node
    [field : SerializeField] public List<Transform> nodes { get; private set; }

    private void Start()
    {
        stateMachine = new MonsterStateMachine(this);

        var _locoState = new MonsterLocoState(stateMachine);
        var _battleState = new MonsterBattleState(stateMachine);

        stateMachine.AddTransition(_locoState, _battleState, new FuncPredicate(() => isFoundPlayer));
        stateMachine.AddAnyTransition(_battleState, new FuncPredicate(() => !isFoundPlayer));
        //Loco State
        //Find State
        //Battle State
        stateMachine.SetState(_locoState);

        navAI = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        stateMachine.Update();
        isFoundPlayer = IsPlayerInView();
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
    private bool IsPlayerInView()
    {
        if (isFoundPlayer) return true;

        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereRad, layerMask : targetMask);

        if (colliders.Length == 0) return false;

        Vector3 _rightDir = ConvertVector3Degree(transform.eulerAngles.y + viewDegree * 0.5f);
        Vector3 _leftDir = ConvertVector3Degree(transform.eulerAngles.y - viewDegree * 0.5f);

        Debug.DrawRay(transform.position, _rightDir * sphereRad, Color.green);
        Debug.DrawRay(transform.position, _leftDir * sphereRad, Color.green);
        

        foreach (var _target in colliders)
        {
            /*
             * 타겟의 위치가 각도 내에 있는지 확인하기
             */
            
            Vector3 dirTarget = _target.transform.position - transform.position;

            if (Vector3.Angle(transform.forward, dirTarget.normalized) < viewDegree * 0.5f)
            {
                Debug.DrawRay(transform.position, dirTarget, Color.red);
                if (Physics.Raycast(transform.position, dirTarget.normalized, out RaycastHit hitInfo, 
                    Vector3.Distance(transform.position, _target.transform.position),targetMask))
                {
                    player = hitInfo.transform;
                    isFoundPlayer = true;
                }
            }

        }

        return false;
    }

    private Vector3 ConvertVector3Degree(float degree)
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
