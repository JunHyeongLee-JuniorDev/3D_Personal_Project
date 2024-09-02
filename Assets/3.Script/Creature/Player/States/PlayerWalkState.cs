using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.player.m_targetSpeed = stateMachine.player.m_PhysicsData.GroundedData.
                                            BaseSpeed;
        aniFadeDuration = 0.1f;

        base.Enter();
        StartAnimation(stateMachine.player.m_aniData.m_animIDWalk);
    }
}
