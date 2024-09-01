using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovePos : MonoBehaviour
{
    [Header("Default Move Speed")]
    [SerializeField]
    private float m_walkSpeed = 5f;
    [SerializeField]
    private float m_sprintSpeed = 10f;

    [Header("�ӵ� ��ȭ ��ġ")]
    private float m_speedChangeRate = 10f;

    [Header("ĳ���� ���� ȸ�� �ӵ�")]
    [Range(0.0f, 0.3f)]
    private float m_rotationSmoothTime = 0.12f;

    [Header("�ó׸ӽ� ī�޶�")]
    public GameObject cinemachineCamTarget;

    [field : Header("ĳ���� �ִϸ��̼� ������")]
    [field : SerializeField]
    public PlayerAnimationData m_aniData { get; private set; }

    //Unity Components
    private CharacterController m_Controller;
    private Rigidbody m_rigidbody;
    private PlayerinputSystem m_input;
    private Animator m_animator;
    private GameObject m_mainCam;

    //Cinemachine
    private float m_cinemachineTargetYaw;
    private float m_cinemachineTargetPitch;

    //Player
    private float m_speed;
    private float m_animationBlend;
    private float m_targetRotation = 0f;
    private float m_rotationVelocity; // �Ҵ� ���� ���� �������� ������ ���ֱ� ���ؼ�
    private readonly float m_threshold = 0.01f;

    //For Debug
    private bool m_hasAnimator;

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
        m_rigidbody = GetComponent<Rigidbody>();
        m_aniData = new PlayerAnimationData();
        m_aniData.Initialize();

        m_hasAnimator = TryGetComponent<Animator>(out m_animator);
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

        float _currentHorizontalSpeed = new Vector3(m_Controller.velocity.x,
                                                    0f, m_Controller.velocity.z).magnitude;

        float _speedOffset = 0.1f;

        //��ǥ �ӵ����� ����
        if (_currentHorizontalSpeed < _targetSpeed - _speedOffset ||
            _currentHorizontalSpeed > _targetSpeed + _speedOffset)
        {
            m_speed = Mathf.Lerp(_currentHorizontalSpeed,
                                 _targetSpeed * m_input.move.magnitude,
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

        Debug.Log("���� �̵��ӵ� :" + m_speed);
        m_Controller.Move(_targetDirection.normalized * (m_speed * Time.deltaTime));

        if (m_hasAnimator)
        {
            Debug.Log("�ִϸ��̼� ���� �߰� �ʿ�");
        }
    }

    private float clampAngle(float _lfAngle, float _lfMin, float _lfMax)
    {
        if (_lfAngle < -360f) _lfAngle += 360f;
        if (_lfAngle > 360f) _lfAngle -= 360f;
        return Mathf.Clamp(_lfAngle, _lfMin, _lfMax);
    }


    private void cameraRotation()
    {
        //���콺�� �������ٸ�
        if (m_input.look.sqrMagnitude >= m_threshold)
        {
            //mouse input�� Time.deltaTime���� ����������
            // �ٵ� ��???

            m_cinemachineTargetYaw = m_input.look.y * 0.1f;
        }

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