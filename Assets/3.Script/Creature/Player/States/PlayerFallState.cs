using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) {}

    private float currentHorizontalSpeed;
    private Vector2 lastMoveDirection;
    private float lastCamYAngle;
    private InputAction fallAction;

    public override void Enter()
    {
        base.Enter();
        lastMoveDirection = player.m_input.move;
        lastCamYAngle = player.m_mainCam.transform.eulerAngles.y;
        inputActions["Move"].Disable();
        inputActions["Fire"].Disable();
    }

    public override void Update()
    {
        base.Update();

        if (player.m_targetSpeed > 5f)
        {
            player.m_targetSpeed -= 15f * Time.deltaTime;
        }

        else if (player.m_targetSpeed > 0.0f)
            player.m_targetSpeed -= 2.3f * Time.deltaTime;


        Move();
        Gravity();
    }

    public override void Move()
    {
        /*
         * Vector3.magnitude
         * 벡터의 길이(float)를 반환한다. 
         * 이는 두 점사이의 거리를 구하거나 속력을 구할 때 사용할 수 있다.
         */

        float _speedOffset = 0.1f;

        //목표 속도까지 조정
        if (player.m_speed < player.m_targetSpeed - _speedOffset ||
            player.m_speed > player.m_targetSpeed + _speedOffset)
        {
            player.m_speed = Mathf.Lerp(player.m_speed, player.m_targetSpeed, Time.deltaTime * groundData.speedChangeRate);

            // 소수점 3째 자리까지만 계산하기
            // 불필요한 연산 줄이기가 가능
            player.m_speed = Mathf.Round(player.m_speed * 1000f) * 0.001f;
        }

        else
        {
            player.m_speed = player.m_targetSpeed;
        }

        player.m_animationBlend = Mathf.Lerp(player.m_animationBlend, player.m_targetSpeed, Time.deltaTime * groundData.speedChangeRate);
        if (player.m_animationBlend < 0.01f) player.m_animationBlend = 0f;

        Vector3 _inputDirection = new Vector3(lastMoveDirection.x, 0f,
                                              lastMoveDirection.y);

        if (!lastMoveDirection.Equals(Vector3.zero))
        {
            player.m_targetRotation = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg +
                                            lastCamYAngle;

            /*
             * SmoothDampAngle은 시작 float 에서 target float까지 지정된 smoothTime 만큼 변한 값을 반환
             * 여기서 저 ref m_rotationVelocity은 필요없음
             */
            float _rotation = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, player.m_targetRotation, ref player.m_rotationVelocity,
                                     groundData.rotationSmoothTime);

            // 카메라 포지션을 기준으로 전방 회전
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
