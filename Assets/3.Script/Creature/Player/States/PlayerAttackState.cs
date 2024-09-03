using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    int attackIndex = 0;

    public PlayerAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        m_targetSpeed = 0f;
        if (attackIndex > 2)
        {
            attackIndex = 0;
        }

        switch (attackIndex)
        {
            case 0:
        StartAnimation(stateMachine.player.m_aniData._DTaniClip[EPlayerState.ATTACK]);
                break;
        }
        attackIndex += 1;
        base.Enter();
    }

    public override void Exit()
    {
        Debug.Log("Attack State Exit");
        base.Exit();
    }

    public override void Move()
    {

    }
}
