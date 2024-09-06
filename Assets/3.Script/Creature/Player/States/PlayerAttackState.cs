using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }
    private int attackIndex;
    private float attackCounter;
    private readonly float maxComboDeley = 0.5f;

    public override void Enter()
    {
        base.Enter();
        attackIndex = 0;
        attackCounter = 0.0f;

        animator.CrossFade(player.m_aniData._comboClip[attackIndex++], 0.2f);
        inputActions["Fire"].started += OnFire;
        inputActions["Move"].Disable();
        inputActions["Sprint"].Disable();
    }

    public override void Update()
    {
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (isAttackAni(animatorStateInfo.fullPathHash) &&
            animatorStateInfo.normalizedTime >= 1f)
        {
            player.isAttack = false;
            return;
        }

        attackCounter += Time.deltaTime;
    }

    public override void Exit()
    {
        base.Exit();
        attackIndex = 0;
        inputActions["Fire"].started -= OnFire;
        inputActions["Move"].Enable();
        inputActions["Sprint"].Enable();
    }

    private bool isAttackAni(int givenHash)
    {
        for(int i = 0; i < player.m_aniData._comboClip.Length; i++)
        if (givenHash.Equals(player.m_aniData._comboClip[i]))
            {
                return true;
            }

        return false;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (attackCounter >= maxComboDeley)
        {
            Debug.Log("공격한다!");
            animator.CrossFade(player.m_aniData._comboClip[attackIndex++], 0.2f);
            attackCounter = 0.0f;
        }

        if (attackIndex >= player.m_aniData._comboClip.Length)
            attackIndex = 0;
    }
}
