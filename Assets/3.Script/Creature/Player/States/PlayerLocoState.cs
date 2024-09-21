using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocoState : PlayerBaseState
{
    private Timer itemGrabAniTimer;
    private bool isGrabAniOn;

    public PlayerLocoState(PlayerStateMachine stateMachine) : base(stateMachine) 
    {
        Managers.Instance.Inventory.OnDynamicInventoryChanged += GrabItem;
        itemGrabAniTimer = new Timer(1.0f, player);
        isGrabAniOn = false;
    }

    public override void Enter()
    {
        base.Enter();
        animator.CrossFade(DTAniClipID[EPlayerAni.LOCO], 0.25f);
    }

    public override void Update()
    {
        base.Update();
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
        isGrabAniOn = false;
    }

    
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void SetAniBool()
    {
        base.SetAniBool();

        if (isGrabAniOn && itemGrabAniTimer.isEnd)
        {
            isGrabAniOn = false;
            Debug.Log("다시 로코로~");
            animator.CrossFade(DTAniClipID[EPlayerAni.LOCO], 0.2f);
        }
    }

    private void ShieldAni()
    {
        if (Managers.Instance.Inventory.PlayerData.equipments[(int)EEquipmentType.Shield].ItemData == null) return;

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
        itemGrabAniTimer.StartTimer();
        isGrabAniOn = true;
        animator.CrossFade(DTAniClipID[EPlayerAni.GRAB], 0.1f);
    }
}
