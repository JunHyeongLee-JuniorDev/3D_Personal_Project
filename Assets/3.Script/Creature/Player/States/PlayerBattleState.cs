using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBattleState : PlayerBaseState
{
    public PlayerBattleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        player.m_Controller.Move(Vector3.zero);
        animator.CrossFade(DTAniClipID[EPlayerState.BATTLE], 0.2f);
    }

    public override void Update()
    {
        base.Update();
        animator.SetFloat(DTAniParamID[EPlayerAniParam.BATTLEX], player.m_input.move.x);
        animator.SetFloat(DTAniParamID[EPlayerAniParam.BATTLEY], player.m_input.move.y);
        JumpAndGravity();
    }

    public override void Exit()
    {
        base.Exit();
        player.isBattle = false;
    }
}
