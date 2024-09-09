using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerBaseState : IState
{
    public PlayerStateMachine stateMachine; // 디버깅용
    protected readonly PlayerGroundData groundData;
    protected readonly PlayerAirData airData;
    protected PlayerController player;
    protected Animator animator;
    protected Dictionary<EPlayerAni, int> DTAniClipID;
    protected Dictionary<EPlayerAniParam, int> DTAniParamID;
    protected InputActionAsset inputActions;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        player = stateMachine.player;
        groundData = player.m_PhysicsData.GroundedData;
        airData = player.m_PhysicsData.AirData;
        animator = player.m_animator;
        DTAniClipID = player.m_aniData._DTaniClipID;
        DTAniParamID = player.m_aniData._DTaniBoolID;
        inputActions = player.m_playerInput.actions;
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
        SetAniBool();
    }

    public virtual void Move()
    {
        /*
         * Vector3.magnitude
         * 벡터의 길이(float)를 반환한다. 
         * 이는 두 점사이의 거리를 구하거나 속력을 구할 때 사용할 수 있다.
         */

        Vector2 _input = player.m_input.move;

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

        Vector3 _inputDirection = new Vector3(_input.x, 0f,
                                              _input.y);

        if (!_input.Equals(Vector3.zero))
        {
            player.m_targetRotation = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg +
                                            player.m_mainCam.transform.eulerAngles.y;

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

    public virtual void Jump()
    {
        if (player.isGrouded)
        {
            player.m_fallTimeOutDelta = airData.FallTimeout;
            player.isFall = false;

            if (player.m_verticalVelocity < 0.0f)
            {
                player.m_verticalVelocity = -2f;
            }

            if (player.isJump && player.m_jumpTimeOutDelta <= 0.0f)
            {
                //H * -2 * G = 목표까지의 원하는 velocity
                player.m_verticalVelocity = Mathf.Sqrt(airData.JumpHeight * -2f * airData.Gravity);
            }

            if (player.m_jumpTimeOutDelta >= 0.0f)
            {
                player.m_jumpTimeOutDelta -= Time.deltaTime;
            }

        }

        else
        {
            player.isFall = true;
            player.isBattle = false;
            player.isJump = false;
        }
    }


    protected virtual void Gravity()
    {
        if (player.m_verticalVelocity < player.m_terminalVelocity)
        {
            player.m_verticalVelocity += airData.Gravity * Time.deltaTime;
        }
    }

    public virtual void OnHurt()
    {
        throw new System.NotImplementedException();
    }

    public virtual void SetAniBool()
    {
        animator.SetBool(DTAniParamID[EPlayerAniParam.ISGROUND], player.isGrouded);
        animator.SetBool(DTAniParamID[EPlayerAniParam.ISFALL], player.isFall);
    }
}
