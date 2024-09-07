using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {}

    public override void Enter()
    {
        base.Enter();

        if (player.isSprint)
            player.m_animator.CrossFade(player.m_aniData._DTaniClipID[EPlayerState.RUNJUMP], 0.1f);

        else
            player.m_animator.CrossFade(player.m_aniData._DTaniClipID[EPlayerState.JUMP], 0.1f);
    }

    public override void Update()
    {
        base.Update();
        Move();
        Jump();
        Gravity();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
