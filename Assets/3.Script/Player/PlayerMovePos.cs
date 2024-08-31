using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovePos : MonoBehaviour
{
    [Header("Default Move Speed")]
    [SerializeField]
    private float m_walkSpeed = 100f;
    [SerializeField]
    private float m_sprintSpeed = 500f;

    [Header("속도 변화 수치")]
    private float m_speedChangeRate = 10f;
    //Components
    private CharacterController m_Controller;
    private PlayerinputSystem m_input;
    private GameObject m_mainCam;

    //Player
    private Vector3 m_direction;
    private float m_speed;
    private float m_animationBlend;
    private float m_targetRotation = 0f;
    private float m_rotationVelocity;
    private float m_verticalVelocity;
    private float m_terminalVelocity = 53f;

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
        m_input.moveEvent += AssignDirection;

    }

    private void FixedUpdate()
    {
        //Movement();
        Debug.Log(m_direction);
    }

    //public void Movement()
    //{
    //    m_Controller.Move()
    //    m_rb += m_direction * m_walkSpeed * Time.deltaTime;
    //}

    public void AssignDirection(Vector2 _dir)
    {
        Vector3 _Direction = new Vector3(_dir.x, 0f, _dir.y);

        m_direction = transform.forward * _Direction.z + transform.right * _Direction.x;
    }

    private void Move()
    {
        float _targetSpeed = m_input.sprint ? m_sprintSpeed : m_walkSpeed;

        if (m_direction == Vector3.zero) _targetSpeed = 0.0f;


        /*
         * Vector3.magnitude
         * 벡터의 길이(float)를 반환한다. 
         * 이는 두 점사이의 거리를 구하거나 속력을 구할 때 사용할 수 있다.
         */

        float _currentHorizontalSpeed = new Vector3(m_Controller.velocity.x,
                                                    0f, m_Controller.velocity.z).magnitude;

        float _speedOffset = 0.1f;

        //목표 속도까지 조정
        if (_currentHorizontalSpeed < _targetSpeed - _speedOffset ||
            _currentHorizontalSpeed > _targetSpeed + _speedOffset)
        {
            m_speed = Mathf.Lerp(_currentHorizontalSpeed,
                                 _targetSpeed * m_input.move.magnitude,
                                 Time.deltaTime * m_speedChangeRate);

            // 소수점 3째 자리까지만 계산하기
            // 불필요한 연산 줄이기가 가능
            m_speed = Mathf.Round(m_speed * 1000f) * 0.001f;
        }
        /*
        else
        {
            m_speed = _targetSpeed;
        }

        Vector3 _inputDirection = new Vector3(m_input.move.x, 0f,
                                              m_input.move.y);

        if (!m_input.move.Equals(Vector3.zero))
        {
            m_targetRotation = Mathf.Atan2(_inputDirection.x, _inputDirection.y) * Mathf.Rad2Deg +
                                            m_mainCam.transform.eulerAngles.y;

            float _rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, m_targetRotation, ref _ro)
        }*/
    }
}
