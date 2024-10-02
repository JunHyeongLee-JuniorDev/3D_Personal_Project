using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocoState : PlayerBaseState
{
    public PlayerLocoState(PlayerStateMachine stateMachine) : base(stateMachine) 
    {
        Managers.Instance.Inventory.OnDynamicInventoryChanged.AddListener(GrabItem);
    }

    public override void Enter()
    {
        base.Enter();
        inputActions["Skill"].started -= OnSkill;
        inputActions["Skill"].started += OnSkill;
        animator.CrossFade(DTAniClipID[EPlayerAni.LOCO], 0.3f);
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
        
        Move();
        Jump();
        Gravity();
        ShieldAni();

        if (player.isGrouded)
        player.m_animator.SetFloat(DTAniClipID[EPlayerAni.LOCOSPEED], player.m_animationBlend);
    }

    public override void Exit()
    {
        base.Exit();
        inputActions["Skill"].started -= OnSkill;
    }

    
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
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
