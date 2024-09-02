using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.player.m_aniData.m_animIDAttack);
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.player.m_input.isFired = false;
    }

    private IEnumerator CountAttackAni_co()
    {
        yield return new WaitForSeconds(stateMachine.player.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

        Exit();
    }
}
