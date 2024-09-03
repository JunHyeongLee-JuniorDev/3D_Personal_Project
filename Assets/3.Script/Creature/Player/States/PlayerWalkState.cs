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
        m_targetSpeed = stateMachine.player.m_PhysicsData.GroundedData.
                                            BaseSpeed;
        aniFadeDuration = 0.1f;

        StartAnimation(stateMachine.player.m_aniData._DTaniClip[EPlayerState.WALK]);

        base.Enter();
    }

    public override void Update()
    {
        base.Update(); 
    }
}
