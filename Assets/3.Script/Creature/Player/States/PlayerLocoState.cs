using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocoState : PlayerBaseState
{
    public PlayerLocoState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        player.m_targetSpeed = 0f;
        animator.CrossFade(DTAniClipID[EPlayerState.LOCO], 0.3f);
        animator.SetFloat(DTAniClipID[EPlayerState.LOCOSPEED], 0f);
        base.Enter();
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

        player.Move();
        player.JumpAndGravity();
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
