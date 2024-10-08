using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using Unity.VisualScripting;
using System.Linq;

public class PlayerBattleState : PlayerBaseState
{
    private float battleAniX;
    private float battleAniY;
    private Collider nearlistEnemy;
    private Vector3 dirToTarget;
    private Vector3 defualtCamPos = new Vector3(0.0f,1.5f,0.0f);

    public PlayerBattleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) 
    {
        player.lockOnUI.SetActive(false);
        inputActions["OnRoll"].started -= OnRoll;
        inputActions["OnRoll"].started += OnRoll;
    }

    public override void Enter()
    {
        nearlistEnemy = null;
        AssignTarget(true, true);

        if (player.m_targetEnemy != null)
        {
            dirToTarget = Vector2.zero;
            player.cinemachineAnimator.Play("TargetCamera");
            if(!player.hurtTimer.isTickin)
            animator.CrossFade(DTAniClipID[EPlayerAni.BATTLE], 0.2f);
            player.lockOnUI.SetActive(true);
            player.thirdPersonCam.enabled = false;

            battleAniX = 0.0f;
            battleAniY = 0.0f;

            player.m_Controller.Move(Vector3.zero);
            inputActions["Look"].started -= OnLook;
            inputActions["Look"].started += OnLook;
            inputActions["OnRoll"].Enable();
            inputActions["Skill"].started -= OnSkill;
            inputActions["Skill"].started += OnSkill;
            inputActions["Sprint"].Disable();
        }
    }

    public override void Update()
    {
        base.Update();
        CancelConditions();

        if (player.m_targetEnemy == null) return;

        dirToTarget = (player.m_targetEnemy.transform.position - player.m_mainCam.transform.position);

        animator.SetFloat(DTAniParamID[EPlayerAniParam.BATTLEX], battleAniX);
        animator.SetFloat(DTAniParamID[EPlayerAniParam.BATTLEY], battleAniY);

        LookAtTarget();
        Move();
        Jump();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        if (player.m_targetEnemy == null) return;

        Vector3 orignPos = dirToTarget;
        orignPos.y = 0.0f;
        PlaceTheCam(orignPos);
    }
    /// <summary>
    /// 플레이어는 타겟을 바라보기에 단순하게 8방향으로만 움직이는 로직, 애니메이션 블렌드 값 갱신
    /// </summary>
    public override void Move()
    {
        if(player.rollBtnTimer.isTickin || player.isSpellCast) return;

        if (player.m_input.move.Equals(Vector2.zero))
        {
            player.m_targetSpeed = 0f;
        }

        else
        {
            player.m_targetSpeed = groundData.BaseSpeed;
        }

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

        if (battleAniX > _input.x + _speedOffset ||
            battleAniX < _input.x - _speedOffset)
        {
            battleAniX = Mathf.Lerp(battleAniX, _input.x, 7.0f * Time.deltaTime);
        }

        else
        {
            battleAniX = _input.x;
        }


        if (battleAniY > _input.y + _speedOffset ||
            battleAniY < _input.y - _speedOffset)
        {
            battleAniY = Mathf.Lerp(battleAniY, _input.y, 10.0f * Time.deltaTime);
        }

        else
        {

            battleAniY = _input.y;
        }

        Vector3 movement3D = player.transform.forward * _input.y + player.transform.right * _input.x;

        if(player.m_Controller.enabled)
        player.m_Controller.Move((movement3D * player.m_speed + new Vector3(0.0f, player.m_verticalVelocity, 0.0f)) * Time.deltaTime);
    }
    /// <summary>
    /// 타겟팅 UI, 카메라 타겟팅을 타겟에 베치하고 플레이어는 Lerp를 사용하여 타겟을 바라봅니다.
    /// </summary>
    private void LookAtTarget()
    {
        player.lockOnUI.transform.position = player.m_targetEnemy.transform.position; // 캔버스의 위치를 타겟 위치로 바꿉니다.
        player.lockOnTarget.position = player.m_targetEnemy.transform.position;// 카메라의 look at을 타겟 위치로 옮깁니다.
        player.lockOnUI.transform.localScale = Vector3.one * 
                                                  (dirToTarget).magnitude * 
                                                   player.lockOnUIScale; // 거리에 비례하여 타겟팅 UI의 크기를 키웁니다.

        //타겟의 y Position이 플레이어의 y포지션과 다르기에 맞춰줍니다.

        Vector3 targetVec = dirToTarget;
        targetVec.y = 0.0f;

        Quaternion targetAngle = Quaternion.LookRotation(targetVec);

        if(!player.rollBtnTimer.isTickin)
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetAngle, player.lookTargetRotLerp * Time.deltaTime);
    }
    /// <summary>
    /// 일정 범위의 OverlapShpere와 카메라의 시야각 내의 객체를 타겟팅하여 PlayerController에 저장합니다.
    /// </summary>
    /// <param name="isInit">초기화 시 시야각의 각의 범위에 상관 없이 화면 중앙에 가장 적 객체를 목표로 합니다.</param>
    /// <param name="isRight">초기화가 아닐 시 화면의 오른쪽 객체 혹은 왼쪽에 있는 객체를 담습니다.</param>
    private void AssignTarget(bool isInit, bool isRight)
    {
        Collider[] enemys = Physics.OverlapSphere(player.transform.position, player.radiusOfView, player.enemyLayer, QueryTriggerInteraction.Ignore);
        // 범위 안에 적이 있다면 배열에 담습니다.

        if (enemys.Length == 0 ||
            isInit && player.m_targetEnemy != null) return;

        float nearlistEnemyDist = -1.0f; // 무조건 거리는 양수이므로 처음에는 음수를 지정해줍니다.

        // For Cashing
        Vector3 _direction;
        Vector3 _enemyPos;
        float _distancePlayerAndEnemy;
        nearlistEnemy = null;
        // For Cashing

        foreach (Collider enemy in enemys)
        {
            _enemyPos = enemy.transform.position;
            if (isInCam(_enemyPos, out Vector3 _enemyViewPos)) // 적이 카메라 내에 있다면
            {
                if (!isInit)
                {
                    if(enemy.Equals(player.m_targetEnemy) || 
                      (isRight && (_enemyViewPos.x < 0.5f)) ||
                      (!isRight && (_enemyViewPos.x > 0.5f))) continue;
                    // 플레이어가 오른쪽 혹은 왼쪽으로 탐색을 요청 적이 범위 밖이라면 continue
                }

                _direction = (_enemyPos - player.transform.position); // 적과 플레이어의 방향 벡터
                _distancePlayerAndEnemy = Vector3.Distance(player.transform.position, _enemyPos); // 적과 플레이어의 거리

                if (!Physics.Raycast(player.m_mainCam.transform.position, _direction.normalized, out RaycastHit hitInfo,
                                _distancePlayerAndEnemy, player.wallAndGroundLayer))// 거리까지 ray를 쏴서 장애물 검사
                {
                    if (_distancePlayerAndEnemy < nearlistEnemyDist ||
                        nearlistEnemyDist < 0.0f) // 가장 가까운 놈이라면 새로운 가까운 놈으로 갱신
                    {
                        nearlistEnemyDist = _distancePlayerAndEnemy;
                        nearlistEnemy = enemy;
                    }
                }
            }
        }

        if (nearlistEnemy != null)
            player.m_targetEnemy = nearlistEnemy;

        else // 위 조건을 만족하는 객체가 없다면 battle state 종료
        {
            if (isInit)
                player.isBattle = false;
        }
    }

    /// <summary>
    /// 일정 마우스의 x delta 값이 1.0 이상이면 타겟 변경(InputAction에서 호출)
    /// </summary>
    /// <param name="x">마우스 x의 delta 값</param>
    private void FindAnotherTarget(float x)
    {
        if (x.Equals(0.0f)) return; // 혹시 모를 예외 처리

        else if (x > 1.0f)
        {
            AssignTarget(false, true);
        }

        else if (x < -1.0f)
        {
            AssignTarget(false, false);
        }
    }

    /// <summary>
    /// 객체가 화면 내에 있다면 true를 반환
    /// </summary>
    /// <param name="position"> 객체의 position</param>
    /// <param name="viewPos"> ViewPort로 변환한 객체 사용을 위해서 out(선택)</param>
    /// <returns></returns>
    private bool isInCam(Vector3 position, out Vector3 viewPos)
    {
        viewPos = player.m_mainCam.WorldToViewportPoint(position);
        /*카메라의 Viewport는 0~1로 정규화 되어있다. 좌측 하단(0, 0), 우측 상단(1, 1) z는 카메라로부터의 객체의 world position이다*/

        if (viewPos.x >= 0 && viewPos.x <= 1
         && viewPos.y >= 0 && viewPos.y <= 1
         && viewPos.z > 0)
            return true;
        
        else return false;
    }
    /// <summary>
    /// player.radiusOfView 변수에 설정된 감지 범위 밖에 있으면 타게팅 취소
    /// </summary>
    /// <returns></returns>
    private bool isItSoFar()
    {
        if (Vector3.Distance(player.transform.position, player.m_targetEnemy.transform.position) > player.radiusOfView) // 객체 감지 범위 밖이라면 취소
            return true;

        else return false;
    }
    /// <summary>
    /// 타겟 방향으로 raycast를 Update에서 발사하여 장애물 검출
    /// </summary>
    /// <returns>장애물이면 true, 아니면 false</returns>
    private bool isBehindTheBuild()
    {
        Debug.DrawRay(player.m_mainCam.transform.position, dirToTarget, Color.red);
        if (Physics.Raycast(player.m_mainCam.transform.position, dirToTarget.normalized,
                           Vector3.Distance(player.m_mainCam.transform.position, player.m_targetEnemy.transform.position),
                           player.wallAndGroundLayer))
            return true;

        else
        return false;
    }
    /// <summary>
    /// 모든 타겟팅 취소 조건(미구현 조건 : 적 객체 사망시)
    /// </summary>
    private void CancelConditions()
    {
        if (player.m_targetEnemy == null ||
            isItSoFar() ||
            isBehindTheBuild())
            player.isBattle = false;
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
    
    /// <summary>
    /// 윌드 좌표에 있는 타게팅 카메라 root를 LateUpdate에서 forward를 타겟 방향으로 설정
    /// </summary>
    /// <param name="originDir"> 캠에서 타겟 방향의 방향 벡터 </param>
    private void PlaceTheCam(Vector3 originDir)
    {
        player.lockOnTargetRoot.transform.forward = Vector3.Lerp(player.lockOnTargetRoot.transform.forward,originDir.normalized, player.lookTargetRotLerp * Time.deltaTime);
        player.thirdPersonCam.m_cinemachineCamTarget.transform.forward = originDir.normalized;
    }

    /// <summary>
    /// 다른 스테이트에서 필요한 기능들 활성화, target 다시 null 화
    /// </summary>
    public override void Exit()
    {
        base.Exit();
        if (!player.isBattle)
        {
            inputActions["Sprint"].Enable();
            player.cinemachineAnimator.Play("FollowCamera");
            player.lockOnUI.SetActive(false);
            player.m_targetEnemy = null;
            player.thirdPersonCam.enabled = true;
            inputActions["Skill"].started -= OnSkill;
        }
        inputActions["OnRoll"].Disable();
        inputActions["Look"].started -= OnLook;
    }
    /// <summary>
    /// 타겟팅 callback 메소드(마우스 "좌, 우"로 발동)
    /// </summary>
    private void OnLook(InputAction.CallbackContext context)
    {
        if (!player.targetBtnTimer.isTickin && Mathf.Abs(context.ReadValue<Vector2>().x) > 1.0f)
        {
            FindAnotherTarget(context.ReadValue<Vector2>().x);
            player.targetBtnTimer.StartTimer(() => { });
        }
    }
    /// <summary>
    /// 스페이스바 = 구르기, 구르기 타이머도 같이 가동
    /// </summary>
    private void OnRoll(InputAction.CallbackContext context)
    {
        if (!player.rollBtnTimer.isTickin && 
            Managers.Instance.Inventory.PlayerData.currentStamina - player.staminaCost >= 0.0f)
        {
            Managers.Instance.Inventory.PlayerData.currentStamina -= player.staminaCost;

            if (Managers.Instance.Inventory.PlayerData.currentStamina < 0.0f)
                Managers.Instance.Inventory.PlayerData.currentStamina = 0.0f;

            PerformRoll();
            inputActions["Move"].Disable();
            inputActions["Fire"].Disable();
            player.OffPlayerHitBox();
            player.m_PlayerData.OnReduceStatus?.Invoke();
            player.staminaFillTimer.StartTimer(() => { });
            player.rollBtnTimer.StartTimer(() =>
            {
                player.OnPlayerHitBox();
                inputActions["Move"].Enable();
                inputActions["Fire"].Enable();

                if (player.isBattle)
                    animator.CrossFade(DTAniClipID[EPlayerAni.BATTLE], 0.3f);

                else
                    animator.CrossFade(DTAniClipID[EPlayerAni.LOCO], 0.3f);
            });
        }
    }

    public override void OnHurt()
    {
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;

        if (_playerData.currentHealth > 0)
        {
            animator.SetLayerWeight(3, 1.0f);
            animator.CrossFade(DTAniClipID[EPlayerAni.Hit], 0.1f);

            player.hurtTimer.StartTimer(() =>
            {
                animator.CrossFade(DTAniClipID[EPlayerAni.BATTLE], 0.25f);
                animator.SetLayerWeight(3, 0.0f);
            });
        }

        else if (!player.isDead)
        {
            player.isDead = true;
            player.m_playerInput.enabled = false;
            player.OnPlayerDead?.Invoke();
            Managers.Instance.Game.ResetGame();
        }
    }
}
