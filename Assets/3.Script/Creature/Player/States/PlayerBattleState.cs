using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBattleState : PlayerBaseState
{
    public PlayerBattleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        inputAction = inputActions["Fire"];
    }

    public override void Enter()
    {
        player.isBattle = true;
        inputAction.started += OnFire;
        animator.Play(DTClipID[EPlayerState.ATTACK],0);
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        inputAction.started -= OnFire;
        player.isBattle = false;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        animator.SetTrigger(player.m_aniData.attackTriggerID);
    }
}
