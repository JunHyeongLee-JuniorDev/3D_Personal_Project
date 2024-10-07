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
        inputActions["OnRoll"].started -= OnRoll;
        inputActions["OnRoll"].started += OnRoll;
    }

    public override void Enter()
    {
        Debug.Log($"player : {this}");
        base.Enter();
        inputActions["Skill"].started -= OnSkill;
        inputActions["Skill"].started += OnSkill;
        inputActions["OnRoll"].Enable();

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
        inputActions["OnRoll"].Disable();
        inputActions["Skill"].started -= OnSkill;
    }

    public override void OnHurt()
    {
        if (player.m_PlayerData.currentHealth > 0)
        {
            animator.SetLayerWeight(3, 1.0f);
            animator.CrossFade(DTAniClipID[EPlayerAni.Hit], 0.1f);

            player.hurtTimer.StartTimer(() =>
            {
                animator.CrossFade(DTAniClipID[EPlayerAni.LOCO], 0.25f);
                animator.SetLayerWeight(3, 0.0f);
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

    private void OnRoll(InputAction.CallbackContext context)
    {
        if (!player.rollBtnTimer.isTickin && 
            player.m_PlayerData.currentStamina <= 0.0f &&
            player.m_PlayerData.currentStamina - player.staminaCost >= 0.0f)
        {
            player.m_PlayerData.currentStamina -= player.staminaCost;

            if (player.m_PlayerData.currentStamina < 0.0f)
                player.m_PlayerData.currentStamina = 0.0f;

            PerformRoll();
            inputActions["Move"].Disable();
            inputActions["Fire"].Disable();
            player.OffPlayerHitBox();

            player.staminaFillTimer.StartTimer(() => { });
            player.rollBtnTimer.StartTimer(() =>
            {
                player.OnPlayerHitBox();
                inputActions["Move"].Enable();
                inputActions["Fire"].Enable();

                if(player.isBattle)
                animator.CrossFade(DTAniClipID[EPlayerAni.BATTLE], 0.3f);

                else
                    animator.CrossFade(DTAniClipID[EPlayerAni.LOCO], 0.3f);
            });
        }
    }

    /// <summary>
    /// 마지막 입력 방향으로 구르기 시전 카메라도 roll 전용 카메라로 잠시 변경
    /// </summary>
    private void PerformRoll()
    {
        // atan2를 사용하면 방향 점 a에서 점 b에 대한 방향 벡터를 구할 수 있다(점 a 기준의) 따라서 이를 라디안에서 각도로 바꾸고 
        // 플레이어 캐릭터 혹은 카메라의 각도 기준의 방향 벡터로 구르기 시전
        // Atan, Atan2는 tan의 역함수로 밑변 대변을 알고 있다면 각 <L을 알 수 있다.
        float rollRotation = Mathf.Atan2(player.m_input.move.x, player.m_input.move.y) * Mathf.Rad2Deg +
                                         player.m_mainCam.transform.eulerAngles.y;
        player.m_Controller.Move(Vector3.zero);
        player.transform.rotation = Quaternion.Euler(0.0f, rollRotation, 0.0f);
        animator.CrossFade(DTAniClipID[EPlayerAni.ROLL], 0.25f);
    }
}
