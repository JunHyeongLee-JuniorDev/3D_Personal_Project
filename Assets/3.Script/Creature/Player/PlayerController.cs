using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: Header("ĳ���� ���� ������")]
    [field: SerializeField]
    public PlayerSO m_PhysicsData { get; private set; }

    [field : Header("ĳ���� �ִϸ��̼� ������")]
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
    private float m_rotationVelocity; // �Ҵ� ���� ���� �������� ������ ���ֱ� ���ؼ�
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
        m_input.player = this; // input���� player�� ���� bool�� ����

        m_StateMachine = new PlayerStateMachine(this);

        m_aniData = new PlayerAnimationData();
        m_aniData.Initialize();// ������ �ʱ�ȭ

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
         * ������ ����(float)�� ��ȯ�Ѵ�. 
         * �̴� �� �������� �Ÿ��� ���ϰų� �ӷ��� ���� �� ����� �� �ִ�.
         */

        Vector2 _input = m_input.move;

        float _currentHorizontalSpeed = new Vector3(m_Controller.velocity.x, 0f, m_Controller.velocity.z).magnitude;

        float _speedOffset = 0.1f;

        //��ǥ �ӵ����� ����
        if (_currentHorizontalSpeed < m_targetSpeed - _speedOffset ||
            _currentHorizontalSpeed > m_targetSpeed + _speedOffset)
        {
            m_speed = Mathf.Lerp(_currentHorizontalSpeed, m_targetSpeed, Time.deltaTime *  m_PhysicsData.GroundedData.speedChangeRate);
            
            // �Ҽ��� 3° �ڸ������� ����ϱ�
            // ���ʿ��� ���� ���̱Ⱑ ����
            m_speed = Mathf.Round(m_speed * 1000f) * 0.001f;
            Debug.Log("���� �ӵ� : " + _currentHorizontalSpeed);
            Debug.Log("��ǥ �ӵ� : " + m_targetSpeed);
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
             * SmoothDampAngle�� ���� float ���� target float���� ������ smoothTime ��ŭ ���� ���� ��ȯ
             * ���⼭ �� ref m_rotationVelocity�� �ʿ����
             */
            float _rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, m_targetRotation, ref m_rotationVelocity,
                                     m_PhysicsData.GroundedData.rotationSmoothTime);

            // ī�޶� �������� �������� ���� ȸ��
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
                //H * -2 * G = ��ǥ������ ���ϴ� velocity
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
                //�������� �ִϸ��̼� ���
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
