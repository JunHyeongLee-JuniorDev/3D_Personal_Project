using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        equipments = Managers.Instance.Inventory.PlayerData.equipments;
    }

    private int[] attackAniClips;
    private int attackIndex = 0;
    private float attackFadeDuration = 0.0f;

    private InventorySlot[] equipments;

    public override void Enter()
    {
        base.Enter();
        if (player.hurtTimer.isTickin || player.rollBtnTimer.isTickin)
        {
            player.isAttack = false;
            return;
        }
            Debug.Log($"player : {this}");
        if (equipments[(int)EEquipmentType.Weapon].StackSize < 0)
        {
            attackAniClips = player.m_aniData.meleeAttackClips;
            player.attackBtnTimer.UpdateMaxTime(player.m_aniData.meleeComboCool);
            attackFadeDuration = player.m_aniData.meleeFadeDuration;
        }
        
        else
        {//클립 추가 요망
            switch (equipments[(int)EEquipmentType.Weapon].Data.weaponType)
            {
                case EWeaponType.None:
                    Debug.LogWarning("정해지지않은 무기 타입");
                    break;
        
                case EWeaponType.SWORD:
                    attackAniClips = player.m_aniData.swordAttackClips;
                    player.attackBtnTimer.UpdateMaxTime(player.m_aniData.swordComboCool);
                    attackFadeDuration = player.m_aniData.swordFadeDuration;
                    break;
        
                case EWeaponType.AXE:
                    attackAniClips = player.m_aniData.axeAttackClips;
                    player.attackBtnTimer.UpdateMaxTime(player.m_aniData.axeComboCool);
                    attackFadeDuration = player.m_aniData.axeFadeDuration;
                    break;
        
                case EWeaponType.MAGIC:
                    attackAniClips = player.m_aniData.magicAttackClips;
                    player.attackBtnTimer.UpdateMaxTime(player.m_aniData.magicComboCool);
                    attackFadeDuration = player.m_aniData.magicFadeDuration;
                    break;
        
                default:
                    Debug.LogWarning("범위를 넘어선 무기 타입");
                    break;
            }
        }
        
        attackIndex = 0;
        player.attackBtnTimer.StartTimer(() => {});
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
        player.isAttack = false;
        inputActions["Fire"].started -= OnFire;
        inputActions["Move"].Enable();
        inputActions["Sprint"].Enable();
        inputActions["Battle"].Enable();
    }

    private void ShowTargettingUI()
    {
        if (player.m_targetEnemy != null)
        {
            player.lockOnUI.transform.position = player.m_targetEnemy.transform.position; // 캔버스의 위치를 타겟 위치로 바꿉니다.
            player.lockOnUI.transform.localScale = Vector3.one *
                                                  (player.m_targetEnemy.transform.position - player.m_mainCam.transform.position).magnitude *
                                                   player.lockOnUIScale; // 거리에 비례하여 타겟팅 UI의 크기를 키웁니다.
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (!player.attackBtnTimer.isTickin)
        {
            if (attackIndex >= attackAniClips.Length)
                attackIndex = 0;
            animator.CrossFade(attackAniClips[attackIndex++], attackFadeDuration);
            player.attackBtnTimer.StartTimer(() => { });
        }
    }

    public override void OnHurt()
    {
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;
        inputActions["Move"].Disable();
        inputActions["Fire"].Disable();

        if (_playerData.currentHealth > 0)
        {
            animator.SetLayerWeight(3, 1.0f);
            animator.CrossFade(DTAniClipID[EPlayerAni.Hit], 0.1f);

            player.hurtTimer.StartTimer(() =>
            {
                animator.SetLayerWeight(3, 0.0f);
                if (player.isBattle)
                    animator.CrossFade(DTAniClipID[EPlayerAni.BATTLE], 0.25f);
                else
                    animator.CrossFade(DTAniClipID[EPlayerAni.LOCO], 0.25f);
            });
        }

        else if (!player.isDead)
        {
            Debug.Log("죽음은 들어오나?");
            player.isDead = true;
            player.m_playerInput.enabled = false;
            player.OnPlayerDead?.Invoke();
            animator.CrossFade(DTAniClipID[EPlayerAni.Death], 0.1f);
            Managers.Instance.Game.ResetGame();
        }
    }
}
