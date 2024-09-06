using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocoState : PlayerBaseState
{
    public PlayerLocoState(PlayerStateMachine stateMachine) : base(stateMachine) 
    {
    }

    public override void Enter()
    {
        base.Enter();
        animator.CrossFade(DTAniClipID[EPlayerState.LOCO], 0.25f);
    }

    public override void Update()
    {
        base.Update();
        if (player.m_input.move.Equals(Vector2.zero))
        {
            player.m_targetSpeed = 0f;
        }

        else
        {
            if (player.isSprint)
            {
                player.m_targetSpeed = groundData.sprintSpeed;
            }

            else
            {
                player.m_targetSpeed = groundData.BaseSpeed;
            }
        }

        Move();
        JumpAndGravity();

        if(player.isGrouded)
        player.m_animator.SetFloat(DTAniClipID[EPlayerState.LOCOSPEED], player.m_animationBlend);
    }

    public override void Exit()
    {
        base.Exit();
    }

    
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
