using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: Header("캐릭터 물리 데이터")]
    [field: SerializeField]
    public PlayerSO m_PhysicsData { get; private set; }

    [field : Header("캐릭터 애니메이션 데이터")]
    [field : SerializeField]
    public PlayerAnimationData m_aniData { get; private set; }

    //Unity Components
    [field : HideInInspector]
    public CharacterController m_Controller;
    public PlayerStateMachine m_StateMachine { get; private set; }
    public PlayerinputSystem m_input { get; private set; }
    public PlayerInput m_playerInput { get; private set; }
    public Animator m_animator { get; private set; }
    public GameObject m_mainCam { get; private set; }

    public bool isSprint;
    public bool isBattle;
    public bool isGrouded;
    public bool isJump;
    public bool debugIsUseGravity = true;

    //Player
    private float m_speed;
    public float m_targetSpeed;
    public float m_animationBlend;
    private float m_targetRotation = 0f;
    private float m_rotationVelocity; // 할당 되지 않은 지역변수 오류를 없애기 위해서
    private float m_verticalVelocity;
    private float m_terminalVelocity = 53.0f;
    private float m_jumpDelta;
    private float m_fallDelta;

    private void Awake()
    {
        if (m_mainCam == null)
        {
            m_mainCam = GameObject.FindGameObjectWithTag("MainCamera");
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

        m_aniData = new PlayerAnimationData();
        m_aniData.Initialize();// 데이터 초기화

        var _locoState = new PlayerLocoState(m_StateMachine);
        var _battleState = new PlayerBattleState(m_StateMachine);
        var _jumpState = new PlayerJumpState(m_StateMachine);

        //transition from to by condition
        

        //AnyTransition
        m_StateMachine.AddAnyTransition(_locoState, new FuncPredicate(() => !isBattle)); //isGrouded && 
        m_StateMachine.AddAnyTransition(_battleState, new FuncPredicate(() => isGrouded && isBattle));
        m_StateMachine.AddAnyTransition(_jumpState, new FuncPredicate(() => !isGrouded && !isBattle));
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
    public void Move()
    {
        /*
         * Vector3.magnitude
         * 벡터의 길이(float)를 반환한다. 
         * 이는 두 점사이의 거리를 구하거나 속력을 구할 때 사용할 수 있다.
         */

        Vector2 _input = m_input.move;

        float _currentHorizontalSpeed = new Vector3(m_Controller.velocity.x, 0f, m_Controller.velocity.z).magnitude;

        float _speedOffset = 0.1f;

        //목표 속도까지 조정
        if (_currentHorizontalSpeed < m_targetSpeed - _speedOffset ||
            _currentHorizontalSpeed > m_targetSpeed + _speedOffset)
        {
            m_speed = Mathf.Lerp(_currentHorizontalSpeed, m_targetSpeed, Time.deltaTime *  m_PhysicsData.GroundedData.speedChangeRate);
            
            // 소수점 3째 자리까지만 계산하기
            // 불필요한 연산 줄이기가 가능
            m_speed = Mathf.Round(m_speed * 1000f) * 0.001f;
            Debug.Log("현재 속도 : " + _currentHorizontalSpeed);
            Debug.Log("목표 속도 : " + m_targetSpeed);
        }

        else
        {
            m_speed = m_targetSpeed;
        }

        m_animationBlend = Mathf.Lerp(m_animationBlend, m_targetSpeed, Time.deltaTime *  m_PhysicsData.GroundedData.speedChangeRate);
        if (m_animationBlend < 0.01f) m_animationBlend = 0f;

        Vector3 _inputDirection = new Vector3(_input.x, 0f,
                                              _input.y);

        if (!_input.Equals(Vector3.zero))
        {
            m_targetRotation = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg +
                                            m_mainCam.transform.eulerAngles.y;

            /*
             * SmoothDampAngle은 시작 float 에서 target float까지 지정된 smoothTime 만큼 변한 값을 반환
             * 여기서 저 ref m_rotationVelocity은 필요없음
             */
            float _rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, m_targetRotation, ref m_rotationVelocity,
                                     m_PhysicsData.GroundedData.rotationSmoothTime);

            // 카메라 포지션을 기준으로 전방 회전
            transform.rotation = Quaternion.Euler(0f, _rotation, 0f);
        }

            

        Vector3 _targetDirection = Quaternion.Euler(0f, m_targetRotation, 0f) * Vector3.forward;

        m_Controller.Move(_targetDirection.normalized * m_speed * Time.deltaTime +
                            new Vector3(0.0f, m_verticalVelocity, 0.0f) * Time.deltaTime);

    }

    public void JumpAndGravity()
    {
        if (!debugIsUseGravity) return;

        if (isGrouded)
        {
            m_fallDelta = m_PhysicsData.AirData.FallTimeout;

            if (m_verticalVelocity < 0.0f)
            {
                m_verticalVelocity = 0.0f;
            }

            if (isJump && m_jumpDelta <= 0.0f)
            {
                //H * -2 * G = 목표까지의 원하는 velocity
                m_verticalVelocity = Mathf.Sqrt(m_PhysicsData.AirData.JumpHeight * -2f * m_PhysicsData.AirData.Gravity);
            }

            if (m_jumpDelta >= 0.0f)
            {
                m_jumpDelta -= Time.deltaTime;
            }
        }

        else
        {
            m_jumpDelta = m_PhysicsData.AirData.JumpTimeout;

            if (m_fallDelta >= 0.0f)
            {
                m_fallDelta -= Time.deltaTime;
            }

            else
            {
                //떨어지는 애니메이션 재생
            }

            isJump = false;
        }

        if (m_verticalVelocity > m_PhysicsData.AirData.Gravity + m_PhysicsData.AirData.GravityOffset)
            m_verticalVelocity += m_PhysicsData.AirData.Gravity * Time.deltaTime;

        else if (m_verticalVelocity < m_PhysicsData.AirData.Gravity - m_PhysicsData.AirData.GravityOffset)
            m_verticalVelocity -= m_PhysicsData.AirData.Gravity * Time.deltaTime;

        else
            m_verticalVelocity = m_PhysicsData.AirData.Gravity;

        Debug.Log(m_verticalVelocity);
    }

    private void GroundedCheck()
    {
        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y + m_PhysicsData.AirData.groundedOffset,
                                        transform.position.z);

        isGrouded = Physics.CheckSphere(spherePos, m_PhysicsData.AirData.groundedRadius, m_PhysicsData.AirData.groundLayers, QueryTriggerInteraction.Ignore);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y + m_PhysicsData.AirData.groundedOffset, transform.position.z),
                m_PhysicsData.AirData.groundedRadius);
    }
}
