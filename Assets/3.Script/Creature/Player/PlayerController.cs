using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [field: Header("캐릭터 물리 데이터")]
    [field: SerializeField]
    public PlayerSO m_PhysicsData { get; private set; }
    public PlayerData m_PlayerData { get; private set; }

    [Header("Player Physics Data Path")]
    [SerializeField]
    private string playerDataPath = "ScriptableObj/PlayerData";
    public PlayerAnimationDataBase m_aniData { get; private set; }

    public readonly float staminaCost = 20.0f;
    public readonly float skillCost = 20.0f;

    //For Debug
    [field : SerializeField] public float lookTargetRotLerp { get; private set; }
    [field: SerializeField] public float staminaFillLerp { get; private set; }


    //Events
    public UnityAction OnPlayerDead;

    //Unity Components
    [field: HideInInspector]
    public CharacterController m_Controller;
    public PlayerStateMachine m_StateMachine { get; private set; }
    public PlayerinputSystem m_input { get; private set; }
    public PlayerInput m_playerInput { get; private set; }
    public Animator m_animator { get; private set; }
    public Camera m_mainCam { get; private set; }
    public CinemachineStateDrivenCamera stateDrivenCamera { get; private set; }
    public ThirdPersonCam thirdPersonCam { get; private set; }
    public Animator cinemachineAnimator;
    public WeaponManager weaponManager { get; private set; }
    [SerializeField] private Collider hitBox;

    //State Bool
    [HideInInspector]public bool isSprint;
    [HideInInspector]public bool isBattle;
    [HideInInspector]public bool isAttack;
    [HideInInspector]public bool isGrouded;
    [HideInInspector]public bool isFall;
    [HideInInspector]public bool isJump;
    [HideInInspector]public bool isRightClicked;
    [HideInInspector]public bool isDrinkPotion;
    [HideInInspector]public bool isSpellCast;
    [HideInInspector]public bool isDead;

    //Timer
    public Timer targetBtnTimer { get; private set; }
    public Timer rollBtnTimer { get; private set; }
    public Timer attackBtnTimer { get; private set; }
    public Timer itemGrabAniTimer { get; private set; }
    public Timer potionTimer { get; private set; }
    public Timer skillTimer { get; private set; }
    public Timer buffTimer { get; private set; }
    public Timer hurtTimer { get; private set; }
    public Timer staminaFillTimer { get; private set; }

    //Player--------------------------------------------------------------------------------
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

    public Vector3 targetDir;
    public Quaternion targetRot;

    //SerializeField
    [Range(0.0f, 25.0f)]
    public float m_blockAniBlend = 5.0f;
    //Player--------------------------------------------------------------------------------

    //Prefabs
    public GameObject lockOnUI { get; private set; }
    public float lockOnUIScale { get; private set; } = 0.01f;
    public Transform lockOnTarget { get; private set; }
    public Transform lockOnTargetRoot { get; private set; }
    public FakeSlider_UI hpSlider { get; private set; }
    public FakeSlider_UI spSlider { get; private set; }
    public FakeSlider_UI apSlider { get; private set; }

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
            m_mainCam = Instantiate(Resources.Load<GameObject>("Prefabs/Player/Main Camera")).GetComponent<Camera>();
            m_mainCam.transform.position = transform.position;
        }

        if (m_PhysicsData == null)
        {
            m_PhysicsData = Resources.Load<PlayerSO>(playerDataPath);
        }

        cinemachineAnimator = m_mainCam.GetComponentInChildren<Animator>();
        stateDrivenCamera = m_mainCam.GetComponentInChildren<CinemachineStateDrivenCamera>();
    }

    private void Start()
    {
        //컴포넌트 캐싱
        m_Controller = GetComponent<CharacterController>();
        m_input = GetComponent<PlayerinputSystem>();
        m_animator = GetComponent<Animator>();
        m_playerInput = GetComponent<PlayerInput>();
        thirdPersonCam = GetComponent<ThirdPersonCam>();
        weaponManager = GetComponentInChildren<WeaponManager>();
        OnPlayerDead -= OnDeathState;
        OnPlayerDead += OnDeathState;
        //컴포넌트 캐싱

        //스테이트 & 타이머 초기화
        m_input.player = this; // input에서 player의 상태 bool값 변경
        m_StateMachine = new PlayerStateMachine(this);
        targetBtnTimer = new Timer(0.5f, this);
        rollBtnTimer = new Timer(0.8f, this);
        attackBtnTimer = new Timer(0.5f, this);
        itemGrabAniTimer = new Timer(0.8f, this);
        potionTimer = new Timer(1.0f, this);
        skillTimer = new Timer(1.0f, this);
        buffTimer = new Timer(10.0f, this);
        hurtTimer = new Timer(0.7f, this);
        staminaFillTimer = new Timer(1.5f, this);
        m_aniData = new PlayerAnimationDataBase();
        m_aniData.Initialize();// 데이터 초기화
        m_Controller.isTrigger = false;
        hitBox.enabled = true;

        m_Controller.enabled = false;
        m_PlayerData = Managers.Instance.Inventory.PlayerData;
        gameObject.transform.position = m_PlayerData.playerPosition;
        gameObject.transform.rotation = m_PlayerData.playerRotation;
        m_Controller.enabled = true;

        //프리펩 생성
        if (lockOnTarget == null)
        lockOnTarget = new GameObject("EnemyTarget_Locator").transform;

        if (lockOnTargetRoot == null)
        {
            lockOnTargetRoot = Instantiate(Resources.Load<GameObject>("Prefabs/Player/TargetCamRoot")).transform;
            lockOnTargetRoot.name = "TargetCamRoot";
            lockOnTargetRoot.GetComponent<PlayerFollowRoot>().AssignPlayer(this);
        }

        if (lockOnUI == null)
        {
            lockOnUI = Instantiate(Resources.Load<GameObject>("Prefabs/Player/Dot"));
            lockOnUI.name = "LockOnUI";
        }

        foreach (var camera in stateDrivenCamera.ChildCameras)
        {//카메라 할당
            if (camera.name.Equals("targetCamera"))
                camera.Follow = lockOnTargetRoot;

            if (camera.name.Equals("PlayerFollowCamera"))
                camera.Follow = transform.GetChild(0);
        }
        Managers.Instance.Game.AssignCamNoise();
        //프리펩 생성

        //각 State 생성
        var _locoState = new PlayerLocoState(m_StateMachine);
        var _attackState = new PlayerAttackState(m_StateMachine);
        var _battleState = new PlayerBattleState(m_StateMachine);
        //var _jumpState = new PlayerJumpState(m_StateMachine);
        var _fallState = new PlayerFallState(m_StateMachine);
        var _deadState = new PlayerDeadState(m_StateMachine);
        
        //AnyTransition
        //m_StateMachine.AddAnyTransition(_jumpState, new FuncPredicate(() => isJump && !isBattle && !isFall));
        m_StateMachine.AddAnyTransition(_locoState, new FuncPredicate(() => !isBattle && isGrouded && !isAttack && !isDead));
        m_StateMachine.AddAnyTransition(_fallState, new FuncPredicate(() => !isGrouded && isFall && !isDead));
        m_StateMachine.AddAnyTransition(_battleState, new FuncPredicate(() => isGrouded && isBattle && !isAttack && !isDead));
        m_StateMachine.AddAnyTransition(_attackState, new FuncPredicate(() => isGrouded &&  isAttack && !isDead));
        m_StateMachine.AddAnyTransition(_deadState, new FuncPredicate(() => isDead));
        m_StateMachine.SetState(_locoState);
    }

    private void Update()
    {
        m_StateMachine.Update();
        StaminaControl();
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

    private void OnDestroy()
    {
        foreach (var transition in m_StateMachine.anyTransitions)
        {
            transition.To.Clear();
        }
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
    public void OffPlayerHitBox()
    {
        hitBox.enabled = false;
    }

    public void OnPlayerHitBox()
    {
        hitBox.enabled = true;
    }

    public void OnNormalAttackCol()
    {
        weaponManager.OnNormalCol();
    }

    public void OffNormalAttackCol()
    {
        weaponManager.OffNormalCol();
    }

    private void OnDeathState()
    {
        CancelAllConditions();
        m_input.enabled = false;
        m_playerInput.enabled = false;
        hitBox.enabled = false;
        Destroy(gameObject, 3.0f);
    }

    private void StaminaControl()
    {
        if (staminaFillTimer.isTickin || isSprint ||
            m_PlayerData.currentStamina == m_PlayerData.maxStamina) return;

        else if (m_PlayerData.currentStamina >= m_PlayerData.maxStamina - 0.5f)
            m_PlayerData.currentStamina = m_PlayerData.maxStamina;

        else
            m_PlayerData.currentStamina = Mathf.Lerp(m_PlayerData.currentStamina, m_PlayerData.maxStamina, staminaFillLerp * Time.deltaTime);

        Managers.Instance.Game.OnStaminaChange?.Invoke();
    }

    public void PlayerFootSound(AnimationEvent animationEvent)
    {

        if(animationEvent.animatorClipInfo.weight >= 0.5f)
        {
            switch (UnityEngine.Random.Range(1,5))
            {
                case 1:
                    Managers.Instance.Sound.Play3DSound("Player/FootStep1", transform.position);
                    break;
                case 2:
                    Managers.Instance.Sound.Play3DSound("Player/FootStep2", transform.position);
                    break;
                case 3:
                    Managers.Instance.Sound.Play3DSound("Player/FootStep3", transform.position);
                    break;
                case 4:
                    Managers.Instance.Sound.Play3DSound("Player/FootStep4", transform.position);
                    break;
            }
        }
    }

    public void CancelAllConditions()
    {
        isAttack = false;
        isSprint = false;
        isBattle = false;
    }
}
