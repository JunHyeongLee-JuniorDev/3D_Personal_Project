using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    protected float aniFadeDuration = 0.1f;

    //Player
    protected float m_speed;
    protected float m_targetSpeed;
    protected float m_animationBlend;
    protected float m_targetRotation = 0f;
    protected float m_rotationVelocity; // 할당 되지 않은 지역변수 오류를 없애기 위해서


    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        groundData = stateMachine.player.m_PhysicsData.GroundedData;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {
        
    }


    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
        HandleInput();
        Move();
    }

    public virtual void HandleInput()
    {
        ReadMoveInput();
    }

    public virtual void Move()
    {
        /*
         * Vector3.magnitude
         * 벡터의 길이(float)를 반환한다. 
         * 이는 두 점사이의 거리를 구하거나 속력을 구할 때 사용할 수 있다.
         */
        PlayerController _player = stateMachine.player;
        Vector2 _input = stateMachine.MovementInput;

        float _currentHorizontalSpeed = new Vector3(_player.m_Controller.velocity.x, 0f, _player.m_Controller.velocity.z).magnitude;

        float _speedOffset = 0.1f;

        //목표 속도까지 조정
        if (_currentHorizontalSpeed < m_targetSpeed - _speedOffset ||
            _currentHorizontalSpeed > m_targetSpeed + _speedOffset)
        {
            m_speed = Mathf.Lerp(_currentHorizontalSpeed,
                                 m_targetSpeed,
                                 Time.deltaTime * _player.m_PhysicsData.GroundedData.speedChangeRate);

            // 소수점 3째 자리까지만 계산하기
            // 불필요한 연산 줄이기가 가능
            m_speed = Mathf.Round(m_speed * 1000f) * 0.001f;

        }

        else
        {
            m_speed = m_targetSpeed;
        }

        m_animationBlend = Mathf.Lerp(m_animationBlend, m_targetSpeed, Time.deltaTime * _player.m_PhysicsData.GroundedData.speedChangeRate);
        if (m_animationBlend < 0.01f) m_animationBlend = 0f;

        Vector3 _inputDirection = new Vector3(_input.x, 0f,
                                              _input.y);

        if (!_input.Equals(Vector3.zero))
        {
            m_targetRotation = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg +
                                            _player.m_mainCam.transform.eulerAngles.y;

            /*
             * SmoothDampAngle은 시작 float 에서 target float까지 지정된 smoothTime 만큼 변한 값을 반환
             * 여기서 저 ref m_rotationVelocity은 필요없음
             */
            float _rotation = Mathf.SmoothDampAngle(_player.transform.eulerAngles.y, m_targetRotation, ref m_rotationVelocity,
                                    _player.m_PhysicsData.GroundedData.rotationSmoothTime);

            // 카메라 포지션을 기준으로 전방 회전
            _player.transform.rotation = Quaternion.Euler(0f, _rotation, 0f);
        }

        Vector3 _targetDirection = Quaternion.Euler(0f, m_targetRotation, 0f) * Vector3.forward;

        _player.m_Controller.Move(_targetDirection.normalized * (m_speed * Time.deltaTime));
    }

    private void ReadMoveInput()
    {
        stateMachine.MovementInput =
            stateMachine.player.m_input.move;
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.player.m_animator.CrossFade(animationHash, aniFadeDuration, 0);
    }

    public virtual void OnHurt()
    {
        throw new System.NotImplementedException();
    }
}
