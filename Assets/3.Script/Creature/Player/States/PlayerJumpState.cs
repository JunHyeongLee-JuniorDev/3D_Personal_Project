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
        player.Move();
        player.JumpAndGravity();
    }

    public override void Exit()
    {
        Debug.Log("플레이는 되니? jump State 야?");
        player.m_animator.Play(player.m_aniData._DTaniClipID[EPlayerState.LAND]);
        base.Exit();
    }
}
