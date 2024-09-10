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

    private void ShowTargettingUI()
    {
        if (player.m_targetEnemy != null)
        {
            player.lockOnCanvas.transform.position = player.m_targetEnemy.transform.position; // 캔버스의 위치를 타겟 위치로 바꿉니다.
            player.lockOnCanvas.transform.localScale = Vector3.one *
                                                  (player.m_targetEnemy.transform.position - player.m_mainCam.transform.position).magnitude *
                                                   player.lockOnCanvasScale; // 거리에 비례하여 타겟팅 UI의 크기를 키웁니다.
        }
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
            animator.CrossFade(player.m_aniData._comboClip[attackIndex++], 0.2f);
            attackCounter = 0.0f;
        }

        if (attackIndex >= player.m_aniData._comboClip.Length)
            attackIndex = 0;
    }
}
