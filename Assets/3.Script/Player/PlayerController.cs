using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: Header("캐릭터 데이터")]
    [field: SerializeField]
    public PlayerSO m_PhysicsData { get; private set; }

    [field : Header("캐릭터 애니메이션 데이터")]
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
    private float m_rotationVelocity; // 할당 되지 않은 지역변수 오류를 없애기 위해서

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
         * 벡터의 길이(float)를 반환한다. 
         * 이는 두 점사이의 거리를 구하거나 속력을 구할 때 사용할 수 있다.
         */

        float _currentHorizontalSpeed = new Vector3(m_Controller.velocity.x, 0f, m_Controller.velocity.z).magnitude;

        float _speedOffset = 0.1f;

        //목표 속도까지 조정
        if (_currentHorizontalSpeed < m_targetSpeed - _speedOffset ||
            _currentHorizontalSpeed > m_targetSpeed + _speedOffset)
        {
            m_speed = Mathf.Lerp(_currentHorizontalSpeed,
                                 m_targetSpeed,
                                 Time.deltaTime * m_PhysicsData.GroundedData.speedChangeRate);

            // 소수점 3째 자리까지만 계산하기
            // 불필요한 연산 줄이기가 가능
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
             * SmoothDampAngle은 시작 float 에서 target float까지 지정된 smoothTime 만큼 변한 값을 반환
             * 여기서 저 ref m_rotationVelocity은 필요없음
             */
            float _rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, m_targetRotation, ref m_rotationVelocity,
                                    m_PhysicsData.GroundedData.rotationSmoothTime);

            // 카메라 포지션을 기준으로 전방 회전
            transform.rotation = Quaternion.Euler(0f, _rotation, 0f);
        }

        Vector3 _targetDirection = Quaternion.Euler(0f, m_targetRotation, 0f) * Vector3.forward;

        m_Controller.Move(_targetDirection.normalized * (m_speed * Time.deltaTime));

        
            Debug.Log("애니메이션 로직 추가 필요");
    }
}
