using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: Header("캐릭터 물리 데이터")]
    [field: SerializeField]
    public PlayerSO m_PhysicsData { get; private set; }

    [Header("Player Physics Data Path")]
    [SerializeField]
    private string playerDataPath = "ScriptableObj/PlayerData";
    public PlayerAnimationDataBase m_aniData { get; private set; }

    //Unity Components
    [field: HideInInspector]
    public CharacterController m_Controller;
    public PlayerStateMachine m_StateMachine { get; private set; }
    public PlayerinputSystem m_input { get; private set; }
    public PlayerInput m_playerInput { get; private set; }
    public Animator m_animator { get; private set; }
    public GameObject m_mainCam { get; private set; }

    //State Bool
    public bool isSprint;
    public bool isBattle;
    public bool isAttack;
    public bool isGrouded;
    public bool isFall;
    public bool isJump;

    //Player
    [HideInInspector]
    public float m_speed;
    [HideInInspector]
    public float m_targetSpeed;
    [HideInInspector]
    public float m_animationBlend;
    [HideInInspector]
    public float m_targetRotation = 0f;
    [HideInInspector]
    public float m_rotationVelocity; // 할당 되지 않은 지역변수 오류를 없애기 위해서
    [HideInInspector]
    public float m_verticalVelocity;
    [HideInInspector]
    public float m_terminalVelocity { get; private set; } = 53f;
    [HideInInspector]
    public float m_jumpTimeOutDelta;
    [HideInInspector]
    public float m_fallTimeOutDelta;

    private void Awake()
    {
        if (m_mainCam == null)
        {
            m_mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        }

        if (m_PhysicsData == null)
        {
            m_PhysicsData = Resources.Load<PlayerSO>(playerDataPath);
        }
    }

    private void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_input = GetComponent<PlayerinputSystem>();
        m_animator = GetComponent<Animator>();
        m_playerInput = GetComponent<PlayerInput>();
        m_input.player = this; // input에서 player의 상태 bool값 변경

        m_StateMachine = new PlayerStateMachine(this);

        m_aniData = new PlayerAnimationDataBase();
        m_aniData.Initialize();// 데이터 초기화

        var _locoState = new PlayerLocoState(m_StateMachine);
        var _attackState = new PlayerAttackState(m_StateMachine);
        var _battleState = new PlayerBattleState(m_StateMachine);
        var _jumpState = new PlayerJumpState(m_StateMachine);
        var _fallState = new PlayerFallState(m_StateMachine);
        //transition from to by condition
        

        //AnyTransition
        m_StateMachine.AddAnyTransition(_locoState, new FuncPredicate(() => !isBattle && isGrouded && !isAttack));
        m_StateMachine.AddAnyTransition(_jumpState, new FuncPredicate(() => !isGrouded && !isBattle && !isFall));
        m_StateMachine.AddAnyTransition(_fallState, new FuncPredicate(() => !isGrouded && isFall));
        m_StateMachine.AddAnyTransition(_battleState, new FuncPredicate(() => isGrouded && isBattle));
        m_StateMachine.AddAnyTransition(_attackState, new FuncPredicate(() => isGrouded &&  isAttack));
        m_StateMachine.SetState(_locoState);
    }

    private void Update()
    {
        m_StateMachine.Update();
    }

    private void FixedUpdate()
    {
        GroundedCheck();
        m_StateMachine.PhysicsUpdate();
    }

    private void GroundedCheck()
    {
        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y + m_PhysicsData.AirData.groundedOffset,
                                        transform.position.z);

        isGrouded = Physics.CheckSphere(spherePos, m_PhysicsData.AirData.groundedRadius, m_PhysicsData.AirData.groundLayers, QueryTriggerInteraction.Ignore);
    }

    private void OnDrawGizmos()
    {
        if (m_PhysicsData == null) return;
        
        Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y + m_PhysicsData.AirData.groundedOffset, transform.position.z),
                m_PhysicsData.AirData.groundedRadius);
    }
}
