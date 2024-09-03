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
    protected float m_rotationVelocity; // �Ҵ� ���� ���� �������� ������ ���ֱ� ���ؼ�


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
         * ������ ����(float)�� ��ȯ�Ѵ�. 
         * �̴� �� �������� �Ÿ��� ���ϰų� �ӷ��� ���� �� ����� �� �ִ�.
         */
        PlayerController _player = stateMachine.player;
        Vector2 _input = stateMachine.MovementInput;

        float _currentHorizontalSpeed = new Vector3(_player.m_Controller.velocity.x, 0f, _player.m_Controller.velocity.z).magnitude;

        float _speedOffset = 0.1f;

        //��ǥ �ӵ����� ����
        if (_currentHorizontalSpeed < m_targetSpeed - _speedOffset ||
            _currentHorizontalSpeed > m_targetSpeed + _speedOffset)
        {
            m_speed = Mathf.Lerp(_currentHorizontalSpeed,
                                 m_targetSpeed,
                                 Time.deltaTime * _player.m_PhysicsData.GroundedData.speedChangeRate);

            // �Ҽ��� 3° �ڸ������� ����ϱ�
            // ���ʿ��� ���� ���̱Ⱑ ����
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
             * SmoothDampAngle�� ���� float ���� target float���� ������ smoothTime ��ŭ ���� ���� ��ȯ
             * ���⼭ �� ref m_rotationVelocity�� �ʿ����
             */
            float _rotation = Mathf.SmoothDampAngle(_player.transform.eulerAngles.y, m_targetRotation, ref m_rotationVelocity,
                                    _player.m_PhysicsData.GroundedData.rotationSmoothTime);

            // ī�޶� �������� �������� ���� ȸ��
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
