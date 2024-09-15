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

    public override void Enter()
    {
        base.Enter();
        attackIndex = 0;
        player.meleeBtnTimer.StartTimer();
        animator.CrossFade(player.m_aniData._comboClip[attackIndex++], 0.2f);
        inputActions["Fire"].started += OnFire;
        inputActions["Move"].Disable();
        inputActions["Sprint"].Disable();

        inputActions["Battle"].Disable();
    }

    private void LookEnemy()
    {
        Vector3 targetDir = (player.m_targetEnemy.transform.position - player.transform.position);
        targetDir.y = 0.0f;


        player.transform.forward = Vector3.Lerp(player.transform.forward, targetDir, 0.1f);
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        if (player.m_targetEnemy == null) return;
        LookEnemy();
        Vector3 targetDir = (player.m_targetEnemy.transform.position - player.m_mainCam.transform.position);
        targetDir.y = 0.0f;
        PlaceTheCam(targetDir);
        ShowTargettingUI();
    }
    private void PlaceTheCam(Vector3 originDir)
    {
        player.lockOnTargetRoot.transform.forward = Vector3.Lerp(player.lockOnTargetRoot.transform.forward, originDir.normalized, 0.05f);
    }

    public override void Exit()
    {
        base.Exit();
        attackIndex = 0;
        inputActions["Fire"].started -= OnFire;
        inputActions["Move"].Enable();
        inputActions["Sprint"].Enable();
        inputActions["Battle"].Enable();
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

    public void OnFire(InputAction.CallbackContext context)
    {
        if (player.meleeBtnTimer.isEnd)
        {
            player.meleeBtnTimer.StartTimer();
            animator.CrossFade(player.m_aniData._comboClip[attackIndex++], 0.2f);
        }

        if (attackIndex >= player.m_aniData._comboClip.Length)
            attackIndex = 0;
    }
}
