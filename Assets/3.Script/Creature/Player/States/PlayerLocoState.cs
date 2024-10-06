using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocoState : PlayerBaseState
{
    public PlayerLocoState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        if(!player.isDebugMode)
        Managers.Instance.Inventory.OnDynamicInventoryChanged.AddListener(GrabItem);
    }

    public override void Enter()
    {
        Debug.Log($"player : {this}");
        base.Enter();
        inputActions["Skill"].started -= OnSkill;
        inputActions["Skill"].started += OnSkill;

        if(!player.hurtTimer.isTickin)
        animator.CrossFade(DTAniClipID[EPlayerAni.LOCO], 0.2f);
    }

    public override void Update()
    {
        base.Update();
        if (player.isSpellCast) return;
        if (player.m_input.move.Equals(Vector2.zero))
        {
            player.m_targetSpeed = 0f;
        }

        else
        {
            if (player.isSprint)
            {
                player.m_targetSpeed = groundData.sprintSpeed;
            }

            else
            {
                player.m_targetSpeed = groundData.BaseSpeed;
            }
        }
        
        
        Jump();
        Move();
        ShieldAni();

        if (player.isGrouded)
        player.m_animator.SetFloat(DTAniClipID[EPlayerAni.LOCOSPEED], player.m_animationBlend);
    }

    public override void Exit()
    {
        base.Exit();
        inputActions["Skill"].started -= OnSkill;
    }

    public override void OnHurt()
    {
        base.OnHurt();
        if(Managers.Instance.Inventory.PlayerData.currentHealth > 0)
        {
            player.hurtTimer.StartTimer(() =>
            {
                animator.CrossFade(DTAniClipID[EPlayerAni.LOCO], 0.2f);
            });
        }
    }

    private void ShieldAni()
    {
        //if (Managers.Instance.Inventory.PlayerData.equipments[(int)EEquipmentType.Shield].Data == null) return;

        if (player.isRightClicked)
        {
            player.m_blockWeight = Mathf.Lerp(player.m_blockWeight, 1f, player.m_blockAniBlend * Time.deltaTime);
            animator.SetLayerWeight(1, player.m_blockWeight);
        }

        else
        {
            player.m_blockWeight = Mathf.Lerp(player.m_blockWeight, 0f, player.m_blockAniBlend * Time.deltaTime);
            animator.SetLayerWeight(1, player.m_blockWeight);
        }
    }

    private void GrabItem()
    {
        player.itemGrabAniTimer.StartTimer(() => {});

        animator.SetLayerWeight(2, 1f);
        animator.CrossFade(DTAniClipID[EPlayerAni.GRAB], 0.25f);
    }
}
