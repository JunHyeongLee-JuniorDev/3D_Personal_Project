using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Default Move Speed")]
    [SerializeField]
    private float m_walkSpeed = 50f;
    [SerializeField]
    private float m_sprintSpeed = 100f;

    [Header("�ӵ� ��ȭ ��ġ")]
    private float m_speedChangeRate = 10f;

    [Header("ĳ���� ���� ȸ�� �ӵ�")]
    [Range(0.0f, 0.3f)]
    private float m_rotationSmoothTime = 0.12f;

    [field : Header("ĳ���� �ִϸ��̼� ������")]
    [field : SerializeField]
    public PlayerAnimationData m_aniData { get; private set; }

    //Unity Components
    private CharacterController m_Controller;
    private PlayerinputSystem m_input;
    private Animator m_animator;
    private GameObject m_mainCam;

    //Player
    private float m_speed;
    private float m_animationBlend;
    private float m_targetRotation = 0f;
    private float m_rotationVelocity; // �Ҵ� ���� ���� �������� ������ ���ֱ� ���ؼ�

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
        m_aniData = new PlayerAnimationData();
        m_aniData.Initialize();
    }

    private void Update()
    {
        Move();
    }


    private void Move()
    {
        float _targetSpeed = m_input.sprint ? m_sprintSpeed : m_walkSpeed;

        if (m_input.move.Equals(Vector2.zero)) _targetSpeed = 0.0f;


        /*
         * Vector3.magnitude
         * ������ ����(float)�� ��ȯ�Ѵ�. 
         * �̴� �� �������� �Ÿ��� ���ϰų� �ӷ��� ���� �� ����� �� �ִ�.
         */

        float _currentHorizontalSpeed = new Vector3(m_Controller.velocity.x, 0f, m_Controller.velocity.z).magnitude;

        float _speedOffset = 0.1f;

        //��ǥ �ӵ����� ����
        if (_currentHorizontalSpeed < _targetSpeed - _speedOffset ||
            _currentHorizontalSpeed > _targetSpeed + _speedOffset)
        {
            m_speed = Mathf.Lerp(_currentHorizontalSpeed,
                                 _targetSpeed,
                                 Time.deltaTime * m_speedChangeRate);

            // �Ҽ��� 3° �ڸ������� ����ϱ�
            // ���ʿ��� ���� ���̱Ⱑ ����
            m_speed = Mathf.Round(m_speed * 1000f) * 0.001f;

        }
        
        else
        {
            m_speed = _targetSpeed;
        }

        m_animationBlend = Mathf.Lerp(m_animationBlend, _targetSpeed, Time.deltaTime * m_speedChangeRate);
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
                                    m_rotationSmoothTime);

            // ī�޶� �������� �������� ���� ȸ��
            transform.rotation = Quaternion.Euler(0f, _rotation, 0f);
        }

        Vector3 _targetDirection = Quaternion.Euler(0f, m_targetRotation, 0f) * Vector3.forward;

        m_Controller.Move(_targetDirection.normalized * (m_speed * Time.deltaTime));

        
            Debug.Log("�ִϸ��̼� ���� �߰� �ʿ�");
    }
}

public class PlayerAnimationData
{
    //Animation Hash
    private string m_aniNameWalk = "Walk";
    private string m_aniNameSprint = "Sprint";
    private string m_aniNameIdle = "Idle";
    private string m_aniNameAttack = "Attack";

    public int m_animIDWalk { get; private set; }
    public int m_animIDSprint { get; private set; }
    public int m_animIDIdle { get; private set; }
    public int m_animIDAttack { get; private set; }

    public void Initialize()
    {
        /*
         string ���� �ְ� �޴� ������ �����⿡ hash���� ĳ���Ѵ�.
        string �� �ѱ��ھ� ���ϱ� ������
        �׷��� hash�� �ٷ� ã�� �� �ֵ���
         */
        m_animIDWalk = Animator.StringToHash(m_aniNameWalk);
        m_animIDSprint = Animator.StringToHash(m_aniNameSprint);
        m_animIDIdle = Animator.StringToHash(m_aniNameIdle);
        m_animIDAttack = Animator.StringToHash(m_aniNameAttack);
    }
}