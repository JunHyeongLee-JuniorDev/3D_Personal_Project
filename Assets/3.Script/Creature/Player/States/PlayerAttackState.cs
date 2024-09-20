using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    private string[] attackAniClips;
    private int attackIndex = 0;
    private int maxAttackIndex = 0;
    public override void Enter()
    {
        base.Enter();
        attackIndex = 0;

        if ((Managers.Instance.Inventory.PlayerData.equipments["Weapon"].ItemData == null))
        {
            attackAniClips = player.m_aniData.meleeAtackClips;
            player.attackBtnTimer.UpdateMaxTime(0.5f);
        }

        else
        {
            attackAniClips = (Managers.Instance.Inventory.PlayerData.equipments["weapon"].ItemData as EquipmentData).AnimationClips;
            player.attackBtnTimer.UpdateMaxTime((Managers.Instance.Inventory.PlayerData.equipments["weapon"].ItemData as EquipmentData).ComboBtnCoolTime);
        }

        attackIndex = 0;
        maxAttackIndex = attackAniClips.Length;

        player.attackBtnTimer.StartTimer();
        animator.CrossFade(attackAniClips[attackIndex++], 0.2f);
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
        inputActions["Fire"].started -= OnFire;
        inputActions["Move"].Enable();
        inputActions["Sprint"].Enable();
        inputActions["Battle"].Enable();
    }

    private void ShowTargettingUI()
    {
        if (player.m_targetEnemy != null)
        {
            player.lockOnCanvas.transform.position = player.m_targetEnemy.transform.position; // ĵ������ ��ġ�� Ÿ�� ��ġ�� �ٲߴϴ�.
            player.lockOnCanvas.transform.localScale = Vector3.one *
                                                  (player.m_targetEnemy.transform.position - player.m_mainCam.transform.position).magnitude *
                                                   player.lockOnCanvasScale; // �Ÿ��� ����Ͽ� Ÿ���� UI�� ũ�⸦ Ű��ϴ�.
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (player.attackBtnTimer.isEnd)
        {
          player.attackBtnTimer.StartTimer();
            animator.CrossFade(attackAniClips[attackIndex++], 0.2f);
        }
        
        if (attackIndex >= maxAttackIndex)
            attackIndex = 0;
    }
}
