using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) {}

    private float currentHorizontalSpeed;
    private Vector2 lastMoveDirection;
    private InputAction fallAction;

    public override void Enter()
    {
        base.Enter();
        lastMoveDirection = player.m_input.move;
        inputActions["Move"].Disable();
        inputActions["Fire"].Disable();
    }

    public override void Update()
    {
        player.m_targetSpeed -= Time.deltaTime;
        Move();
        JumpAndGravity();
    }

    public override void Move()
    {
        Vector2 _input = lastMoveDirection;

        float _speedOffset = 0.1f;

        //��ǥ �ӵ����� ����
        if (player.m_speed < player.m_targetSpeed - _speedOffset ||
            player.m_speed > player.m_targetSpeed + _speedOffset)
        {
            player.m_speed = Mathf.Lerp(player.m_speed, player.m_targetSpeed, Time.deltaTime * groundData.speedChangeRate);

            // �Ҽ��� 3° �ڸ������� ����ϱ�
            // ���ʿ��� ���� ���̱Ⱑ ����
            player.m_speed = Mathf.Round(player.m_speed * 1000f) * 0.001f;
        }

        else
        {
            player.m_speed = player.m_targetSpeed;
        }

        player.m_animationBlend = Mathf.Lerp(player.m_animationBlend, player.m_targetSpeed, Time.deltaTime * groundData.speedChangeRate);
        if (player.m_animationBlend < 0.01f) player.m_animationBlend = 0f;

        Vector3 _inputDirection = new Vector3(_input.x, 0f,
                                              _input.y);

        if (!_input.Equals(Vector3.zero))
        {
            player.m_targetRotation = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg +
                                            player.m_mainCam.transform.eulerAngles.y;

            /*
             * SmoothDampAngle�� ���� float ���� target float���� ������ smoothTime ��ŭ ���� ���� ��ȯ
             * ���⼭ �� ref m_rotationVelocity�� �ʿ����
             */
            float _rotation = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, player.m_targetRotation, ref player.m_rotationVelocity,
                                     groundData.rotationSmoothTime);

            // ī�޶� �������� �������� ���� ȸ��
            player.transform.rotation = Quaternion.Euler(0f, _rotation, 0f);
        }



        Vector3 _targetDirection = Quaternion.Euler(0f, player.m_targetRotation, 0f) * Vector3.forward;

        player.m_Controller.Move(_targetDirection.normalized * player.m_speed * Time.deltaTime +
                            new Vector3(0.0f, player.m_verticalVelocity, 0.0f) * Time.deltaTime);
    }

    public override void Exit()
    {
        base.Exit();
        inputActions["Move"].Enable();
        inputActions["Fire"].Enable();
    }
}
