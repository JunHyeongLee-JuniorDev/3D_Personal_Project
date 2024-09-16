using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.Rendering.LookDev;
using Cinemachine;

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
    public Camera m_mainCam { get; private set; }
    public ThirdPersonCam thirdPersonCam { get; private set; }
    public Animator cinemachineAnimator;

    //State Bool
    public bool isSprint;
    public bool isBattle;
    public bool isAttack;
    public bool isGrouded;
    public bool isFall;
    public bool isJump;
    public bool isRightClicked;

    //Timer
    public Timer targetBtnTimer { get; private set; }
    public Timer rollBtnTimer { get; private set; }

    public Timer meleeBtnTimer { get; private set; }

    //Player
    //Hide
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
    [HideInInspector]
    [Range(0.0f, 1.0f)]
    public float m_blockWeight;

    //SerializeField
    [Range(0.0f, 25.0f)]
    public float m_blockAniBlend = 5.0f;
    //Player

    //Prefabs
    [field: Header("프리팹")]
    [field: SerializeField]
    public GameObject lockOnCanvas { get; private set; }
    public float lockOnCanvasScale { get; private set; } = 0.5f;
    [field: SerializeField]
    public Transform lockOnTarget { get; private set; }
    [field: SerializeField]
    public Transform lockOnTargetRoot { get; private set; }

    //Enemy Targetting System
    //***************************************************************************
    [field : SerializeField]
    public float radiusOfView { get; private set; }
    [field: SerializeField]
    public LayerMask enemyLayer { get; private set; }
    [field: SerializeField]
    public LayerMask wallAndGroundLayer { get; private set; }
    //***************************************************************************

    //In Scene
    public Collider m_targetEnemy;
    private void Awake()
    {
        if (m_mainCam == null)
        {
            m_mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            cinemachineAnimator = m_mainCam.gameObject.GetComponentInChildren<Animator>();

            if (cinemachineAnimator == null)
                Debug.Log("시네머신 할당 X");
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
        thirdPersonCam = GetComponent<ThirdPersonCam>();
        m_input.player = this; // input에서 player의 상태 bool값 변경
        m_StateMachine = new PlayerStateMachine(this);
        targetBtnTimer = new Timer(0.0f, 0.5f, this);
        rollBtnTimer = new Timer(0.0f, 1.0f, this);
        meleeBtnTimer = new Timer(0.0f, 0.5f, this);

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
        m_StateMachine.AddAnyTransition(_battleState, new FuncPredicate(() => isGrouded && isBattle && !isAttack));
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

    private void LateUpdate()
    {
        m_StateMachine.LateUpdate();
    }

    private void GroundedCheck()
    {
        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y + m_PhysicsData.AirData.groundedOffset,
                                        transform.position.z);

        isGrouded = Physics.CheckSphere(spherePos, m_PhysicsData.AirData.groundedRadius, m_PhysicsData.AirData.groundLayers, QueryTriggerInteraction.Ignore);
    }

    public void OffPlayerAttackAni()
    {
        isAttack = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y + m_PhysicsData.AirData.groundedOffset, transform.position.z),
                m_PhysicsData.AirData.groundedRadius);

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, radiusOfView);
    }
}
