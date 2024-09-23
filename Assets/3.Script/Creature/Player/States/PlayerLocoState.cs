using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocoState : PlayerBaseState
{
    private bool isGrabAniOn;

    public PlayerLocoState(PlayerStateMachine stateMachine) : base(stateMachine) 
    {
        Managers.Instance.Inventory.OnDynamicInventoryChanged.AddListener(GrabItem);
        isGrabAniOn = false;
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
        if (isGrabAniOn || player.isSpellCast) return;
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
        isGrabAniOn = false;
    }

    
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void SetAniBool()
    {
        base.SetAniBool();

        if ((isGrabAniOn && player.itemGrabAniTimer.isEnd) ||
            (player.isSpellCast && player.skillTimer.isEnd))
        {
            isGrabAniOn = false;
            player.isSpellCast = false;
            animator.CrossFade(DTAniClipID[EPlayerAni.LOCO], 0.25f);
        }
    }

    private void ShieldAni()
    {
        if (Managers.Instance.Inventory.PlayerData.equipments[(int)EEquipmentType.Shield].Data == null) return;

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
        player.itemGrabAniTimer.StartTimer();
        isGrabAniOn = true;
        animator.SetLayerWeight(2, 1f);
        animator.CrossFade(DTAniClipID[EPlayerAni.GRAB], 0.25f);
    }

    public void OnSkill(InputAction.CallbackContext context)
    {
        if (context.started &&
         !player.isSpellCast)
        {
            player.isSpellCast = true;
            UseSkill();
        }
    }

    private void UseSkill()
    {
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;

        if (_playerData.equipments[(int)EEquipmentType.Weapon].StackSize > 0)
        {
            switch (_playerData.equipments[(int)EEquipmentType.Weapon].Data.weaponType)
            {
                case EWeaponType.SWORD:
                    player.skillTimer.UpdateMaxTime(1.5f);
                    animator.CrossFade(DTAniClipID[EPlayerAni.SwordSkill], 0.2f);
                    break;

                case EWeaponType.MAGIC:
                    player.skillTimer.UpdateMaxTime(3.5f);
                    animator.CrossFade(DTAniClipID[EPlayerAni.MagicSkill], 0.2f);
                    break;

                case EWeaponType.AXE:
                    player.skillTimer.UpdateMaxTime(3.0f);
                    animator.CrossFade(DTAniClipID[EPlayerAni.AxeSkill], 0.2f);
                    break;
            }

            player.skillTimer.StartTimer();
        }

        else
        {
            player.isSpellCast = false;
            return;
        }
    }
}
