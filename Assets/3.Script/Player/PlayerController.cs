using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: Header("ĳ���� ������")]
    [field: SerializeField]
    public PlayerSO m_PhysicsData { get; private set; }

    [field : Header("ĳ���� �ִϸ��̼� ������")]
    [field : SerializeField]
    public PlayerAnimationData m_aniData { get; private set; }

    //Unity Components
    private CharacterController m_Controller;
    private PlayerStateMachine m_StateMachine;
    public PlayerinputSystem m_input { get; private set; }
    public Animator m_animator { get; private set; }
    private GameObject m_mainCam;

    //Player
    private float m_speed;
    public float  m_targetSpeed;
    private float m_animationBlend;
    private float m_targetRotation = 0f;
    private float m_rotationVelocity; // �Ҵ� ���� ���� �������� ������ ���ֱ� ���ؼ�

    private void Awake()
    {
        if (m_mainCam == null)
        {
            m_mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        }

        m_StateMachine = new PlayerStateMachine(this);
    }

    private void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_input = GetComponent<PlayerinputSystem>();
        m_animator = GetComponent<Animator>();
        m_aniData = new PlayerAnimationData();
        m_aniData.Initialize();

        m_StateMachine.ChangeState(m_StateMachine.idleState);
    }

    private void Update()
    {
        m_StateMachine.Update();
    }

    private void FixedUpdate()
    {
        m_StateMachine.PhysicsUpdate();
    }

    public void Move()
    {
        /*
         * Vector3.magnitude
         * ������ ����(float)�� ��ȯ�Ѵ�. 
         * �̴� �� �������� �Ÿ��� ���ϰų� �ӷ��� ���� �� ����� �� �ִ�.
         */

        float _currentHorizontalSpeed = new Vector3(m_Controller.velocity.x, 0f, m_Controller.velocity.z).magnitude;

        float _speedOffset = 0.1f;

        //��ǥ �ӵ����� ����
        if (_currentHorizontalSpeed < m_targetSpeed - _speedOffset ||
            _currentHorizontalSpeed > m_targetSpeed + _speedOffset)
        {
            m_speed = Mathf.Lerp(_currentHorizontalSpeed,
                                 m_targetSpeed,
                                 Time.deltaTime * m_PhysicsData.GroundedData.speedChangeRate);

            // �Ҽ��� 3° �ڸ������� ����ϱ�
            // ���ʿ��� ���� ���̱Ⱑ ����
            m_speed = Mathf.Round(m_speed * 1000f) * 0.001f;

        }
        
        else
        {
            m_speed = m_targetSpeed;
        }

        m_animationBlend = Mathf.Lerp(m_animationBlend, m_targetSpeed, Time.deltaTime * m_PhysicsData.GroundedData.speedChangeRate);
        if (m_animationBlend < 0.01f) m_animationBlend = 0f;

        Vector3 _inputDirection = new Vector3(m_input.move.x, 0f,
                                              m_input.move.y);

        if (!m_input.move.Equals(Vector3.zero))
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

        m_Controller.Move(_targetDirection.normalized * (m_speed * Time.deltaTime));

        
            Debug.Log("�ִϸ��̼� ���� �߰� �ʿ�");
    }
}
