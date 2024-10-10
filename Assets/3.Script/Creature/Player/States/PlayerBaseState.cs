using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerBaseState : IState
{
    public PlayerStateMachine stateMachine; // 디버깅용
    protected readonly PlayerGroundData groundData;
    protected readonly PlayerAirData airData;
    protected PlayerController player;
    protected Animator animator;
    protected Dictionary<EPlayerAni, int> DTAniClipID;
    protected Dictionary<EPlayerAniParam, int> DTAniParamID;
    protected InputActionAsset inputActions;


    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        player = stateMachine.player;
        groundData = player.m_PhysicsData.GroundedData;
        airData = player.m_PhysicsData.AirData;
        animator = player.m_animator;
        DTAniClipID = player.m_aniData._DTaniClipID;
        DTAniParamID = player.m_aniData._DTaniParamID;
        inputActions = player.m_playerInput.actions;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }


    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
        if (player.isDead) return;
        SetAniBool();
    }

    public virtual void LateUpdate()
    {

    }


    public virtual void Move()
    {
        if (!player.m_Controller.enabled) return;
        /*
         * Vector3.magnitude
         * 벡터의 길이(float)를 반환한다. 
         * 이는 두 점사이의 거리를 구하거나 속력을 구할 때 사용할 수 있다.
         */

        Vector2 _input = player.m_input.move;

        float _speedOffset = 0.1f;

        //목표 속도까지 조정
        if (player.m_speed < player.m_targetSpeed - _speedOffset ||
            player.m_speed > player.m_targetSpeed + _speedOffset)
        {
            player.m_speed = Mathf.Lerp(player.m_speed, player.m_targetSpeed, Time.deltaTime * groundData.speedChangeRate);

            // 소수점 3째 자리까지만 계산하기
            // 불필요한 연산 줄이기가 가능
            player.m_speed = Mathf.Round(player.m_speed * 1000f) * 0.001f;
        }

        else
        {
            player.m_speed = player.m_targetSpeed;
        }

        player.m_animationBlend = Mathf.Lerp(player.m_animationBlend, player.m_targetSpeed, Time.deltaTime * groundData.speedChangeRate);
        if (player.m_animationBlend < 0.01f) player.m_animationBlend = 0f;

        Vector3 _inputDirection = new Vector3(_input.x, 0f,
                                              _input.y);

        if (!_input.Equals(Vector3.zero))
        {
            // Atan, Atan2는 tan의 역함수로 밑변 대변을 알고 있다면 각 <L을 알 수 있다.
            player.m_targetRotation = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg +
                                            player.m_mainCam.transform.eulerAngles.y;

            /*
             * SmoothDampAngle은 시작 float 에서 target float까지 지정된 smoothTime 만큼 변한 값을 반환
             */
            float _rotation = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, player.m_targetRotation, ref player.m_rotationVelocity,
                                     groundData.rotationSmoothTime);

            // 카메라 포지션을 기준으로 전방 회전
            player.transform.rotation = Quaternion.Euler(0f, _rotation, 0f);
            player.targetDir = Quaternion.Euler(0.0f, player.m_targetRotation, 0.0f) * Vector3.forward;

            player.m_Controller.Move(player.targetDir.normalized * player.m_speed * Time.deltaTime +
                                        new Vector3(0.0f, player.m_verticalVelocity, 0.0f) * Time.deltaTime);
        }
    }

    public virtual void Jump()
    {
        if (player.isGrouded)
        {
            player.m_fallTimeOutDelta = airData.FallTimeout;
            player.isFall = false;

            if (player.m_verticalVelocity < 0.0f)
            {
                player.m_verticalVelocity = -9.8f;
            }

            if (player.isJump && player.m_jumpTimeOutDelta >= 0.0f)
            {
                //H * -2 * G = 목표까지의 원하는 velocity
                player.m_verticalVelocity = Mathf.Sqrt(airData.JumpHeight * -2f * airData.Gravity);
            }

            if (player.m_jumpTimeOutDelta >= 0.0f)
            {
                player.m_jumpTimeOutDelta -= Time.deltaTime;
            }
        }

        else
        {
            player.isBattle = false;
            player.m_targetEnemy = null;

            player.m_jumpTimeOutDelta = airData.JumpTimeout;

            if (player.m_fallTimeOutDelta >= 0.0f)
            {
                player.m_fallTimeOutDelta -= Time.deltaTime;
            }

            else
            {
                player.isFall = true;
            }

            Gravity();
            player.isJump = false;

        }
    }


    protected virtual void Gravity()
    {
        player.m_verticalVelocity += airData.Gravity * Time.deltaTime;
    }

    public virtual void OnHurt()
    {
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;
        
        if (_playerData.currentHealth > 0)
        {
            animator.SetLayerWeight(3, 1.0f);
            animator.CrossFade(DTAniClipID[EPlayerAni.Hit], 0.1f);

            player.hurtTimer.StartTimer(() =>
            {
                animator.SetLayerWeight(3, 0.0f);
            });
        }
        
        else if (!player.isDead)
        {
            player.isDead = true;
            player.m_playerInput.enabled = false;
            player.m_playerInput.actions.Disable();
            player.OnPlayerDead?.Invoke();
            animator.CrossFade(DTAniClipID[EPlayerAni.Death], 0.1f);
            Managers.Instance.Game.ResetGame();
        }
    }

    public virtual void SetAniBool()
    {
        animator.SetBool(DTAniParamID[EPlayerAniParam.ISGROUND], player.isGrouded);
    }

    public void OnSkill(InputAction.CallbackContext context)
    {
        if (context.started &&
         !player.isSpellCast &&
         !(Managers.Instance.Inventory.PlayerData.currentMana - player.skillCost <= 0.0f))
        {
            player.isSpellCast = true;
            UseSkill();
        }
    }

    protected virtual void UseSkill()
    {
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;

        _playerData.OnReduceStatus?.Invoke();

        if (_playerData.equipments[(int)EEquipmentType.Weapon].StackSize > 0)
        {
            _playerData.currentMana -= player.skillCost;
            switch (_playerData.equipments[(int)EEquipmentType.Weapon].Data.weaponType)
            {
                case EWeaponType.SWORD:
                    player.skillTimer.UpdateMaxTime(1.5f);
                    Managers.Instance.Sound.Play("Player/PowerUp");
                    animator.CrossFade(DTAniClipID[EPlayerAni.SwordSkill], 0.2f);
                    break;

                case EWeaponType.MAGIC:
                    player.skillTimer.UpdateMaxTime(3.5f);
                    animator.CrossFade(DTAniClipID[EPlayerAni.MagicSkill], 0.2f);
                    break;

                case EWeaponType.AXE:
                    Managers.Instance.Game.CamShakeOn();
                    player.skillTimer.UpdateMaxTime(3.0f);
                    Managers.Instance.Sound.Play("Player/Thunder");
                    animator.CrossFade(DTAniClipID[EPlayerAni.AxeSkill], 0.2f);
                    break;
            }

            player.skillTimer.StartTimer(() =>
            {
                if(_playerData.equipments[(int)EEquipmentType.Weapon].Data.weaponType == EWeaponType.SWORD)
                {
                    float originDamage = _playerData.equipments[(int)EEquipmentType.Weapon].Data.stat;
                    _playerData.equipments[(int)EEquipmentType.Weapon].Data.stat += _playerData.equipments[(int)EEquipmentType.Weapon].Data.stat * 1.2f;
                    player.isSpellCast = false;

                    player.buffTimer.StartTimer(() =>
                    {
                        _playerData.equipments[(int)EEquipmentType.Weapon].Data.stat = originDamage;
                        player.weaponManager.endSkill();
                    });

                    if (player.isBattle)
                        animator.CrossFade(DTAniClipID[EPlayerAni.BATTLE], 0.2f);

                    else
                        animator.CrossFade(DTAniClipID[EPlayerAni.LOCO], 0.2f);

                    return;
                }
                
                Managers.Instance.Game.CamShakeOff();
                player.isSpellCast = false;

                if (player.isBattle)
                    animator.CrossFade(DTAniClipID[EPlayerAni.BATTLE], 0.2f);

                else
                    animator.CrossFade(DTAniClipID[EPlayerAni.LOCO], 0.2f);

                player.weaponManager.endSkill();
            });
            player.weaponManager.actiaveSkill();
        }

        else
        {
            player.isSpellCast = false;
            return;
        }
    }
}

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        Managers.Instance.Sound.Play("Player/PlayerDead");
        Managers.Instance.Sound.Play("Bgm/YouDied");

        player.m_playerInput.actions.Disable();
        animator.CrossFade(DTAniClipID[EPlayerAni.Death], 0.1f);
    }
}