using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class PlayerBattleState : PlayerBaseState
{
    private float battleAniX;
    private float battleAniY;
    private Collider nearlistEnemy;
    private Vector3 dirToTarget;
    private Vector3 defualtCamPos = new Vector3(0.0f,1.5f,0.0f);

    private bool isTargetting = false;
    private bool isRolling = false;
    private float targettingCounter = 0.0f;
    private float maxTargettingCount = 0.5f;
    private float rollCounter = 0.0f;
    private float maxrollcount = 2.0f;

    public PlayerBattleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) {}

    public override void Enter()
    {
        dirToTarget = Vector2.zero;
        nearlistEnemy = null;
        battleAniX = 0.0f;
        battleAniY = 0.0f;
        targettingCounter = 0.0f;
        player.m_Controller.Move(Vector3.zero);
        player.lockOnCanvas.SetActive(true);
        inputActions["Look"].started += OnLook;
        inputActions["Jump"].started += OnRoll;
        player.thirdPersonCam.enabled = false;
        player.cinemachineAnimator.Play("TargetCamera1");
        animator.CrossFade(DTAniClipID[EPlayerAni.BATTLE], 0.2f);
        AssignTarget(true, true);
        isTargetting = false;
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.T))
        {
            player.isBattle = false;
            player.m_targetEnemy.GetComponent<Renderer>().material.color = Color.yellow;
            return;
        }

        CancelConditions();

        if (player.m_targetEnemy == null) return;

        dirToTarget = (player.m_targetEnemy.transform.position - player.m_mainCam.transform.position);

        animator.SetFloat(DTAniParamID[EPlayerAniParam.BATTLEX], battleAniX);
        animator.SetFloat(DTAniParamID[EPlayerAniParam.BATTLEY], battleAniY);

        Move();
        Gravity();
        TargettingCount();
        if (isRolling) RollTimer();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Vector3 orignPos = dirToTarget.normalized;
        orignPos.y = 0.0f;

            player.thirdPersonCam.m_cinemachineCamTarget.transform.forward = Vector3.Lerp(player.thirdPersonCam.m_cinemachineCamTarget.transform.forward,
                                                                                                  orignPos, 0.05f);
        PlaceTheCam(orignPos);
    }

    public override void Move()
    {
        if(isRolling) return;

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
        LookAtTarget();

        player.m_Controller.Move((movement3D * player.m_speed + new Vector3(0.0f, player.m_verticalVelocity, 0.0f)) * Time.deltaTime);
    }

    private void LookAtTarget()
    {
        player.lockOnCanvas.transform.position = player.m_targetEnemy.transform.position; // 캔버스의 위치를 타겟 위치로 바꿉니다.
        player.lockOnTarget.position = player.m_targetEnemy.transform.position;// 카메라의 look at을 타겟 위치로 옮깁니다.
        player.lockOnCanvas.transform.localScale = Vector3.one * 
                                                  (dirToTarget).magnitude * 
                                                   player.lockOnCanvasScale; // 거리에 비례하여 타겟팅 UI의 크기를 키웁니다.

        Vector3 targetPos = new Vector3(player.m_targetEnemy.transform.position.x, player.transform.position.y, player.m_targetEnemy.transform.position.z);
        //타겟의 y Position이 플레이어의 y포지션과 다르기에 맞춰줍니다.
        Quaternion targetAngle = Quaternion.LookRotation(targetPos - player.transform.position);

        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetAngle, 0.05f);
    }
    protected override void Gravity()
    {
        if (player.isGrouded) { player.m_verticalVelocity = -2f; return; }

        else
        {
            player.isFall = true;
            player.isBattle = false;
            player.m_targetEnemy = null;
            player.isJump = false;
        }

        base.Gravity();
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

        if (enemys == null ||
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
        {
            if (player.m_targetEnemy != null) // 디버깅을 위한 이전 객체 체크
                player.m_targetEnemy.GetComponent<Renderer>().material.color = Color.yellow;
            player.m_targetEnemy = nearlistEnemy;
            player.m_targetEnemy.GetComponent<Renderer>().material.color = Color.red;// 디버깅을 위한 다음 객체 체크
        }

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

    private bool isItSoFar()
    {
        if (Vector3.Distance(player.transform.position, player.m_targetEnemy.transform.position) > player.radiusOfView) // 객체 감지 범위 밖이라면 취소
            return true;

        else return false;
    }

    private bool isBehindTheBuild()
    {
        Debug.DrawRay(player.m_mainCam.transform.position, dirToTarget, Color.red);
        if (Physics.Raycast(player.m_mainCam.transform.position, dirToTarget.normalized,
                           Vector3.Distance(player.m_mainCam.transform.position, player.m_targetEnemy.transform.position),
                           player.wallAndGroundLayer))
        {
            return true;
        }

        else return false;
    }

    /// <summary>
    /// 모든 타겟팅 취소 조건(미구현 조건 : 적 객체 사망시, 적이 벽 뒤에 있다면)
    /// </summary>
    private void CancelConditions()
    {
        if (player.m_targetEnemy == null ||
            isItSoFar() ||
            isBehindTheBuild())
            player.isBattle = false;
    }

    private void PerformRoll()
    {
        player.cinemachineAnimator.Play("RollCamera");

        player.transform.forward = player.transform.forward + new Vector3(player.transform.forward.x * player.m_input.move.x,
                                                                          player.transform.forward.y, 
                                                                          player.transform.forward.z * player.m_input.move.y);

        animator.Play(DTAniClipID[EPlayerAni.ROLL]);
    }
    private void RollTimer()
    {
        rollCounter += Time.deltaTime;

        if (rollCounter > maxrollcount)
        {
            player.cinemachineAnimator.Play("TargetCamera1");
            isRolling = false;
            rollCounter = 0.0f;
            animator.CrossFade(DTAniClipID[EPlayerAni.BATTLE], 0.2f);
        }
    }

    private void PlaceTheCam(Vector3 originDir)
    {
        player.lockOnTargetRoll.transform.position = player.transform.position + defualtCamPos;
        player.lockOnTargetRoll.transform.forward = originDir;

        player.thirdPersonCam.m_cinemachineCamTarget.transform.forward = Vector3.Lerp(player.thirdPersonCam.m_cinemachineCamTarget.transform.forward,
                                                                                                      originDir, 0.05f);
    }

    public override void Exit()
    {
        base.Exit();

        if (!player.isBattle)
        {
            Debug.Log("지금 나가는 중");
            player.cinemachineAnimator.Play("FollowCamera");
            player.thirdPersonCam.enabled = true;
            player.lockOnCanvas.SetActive(false);
            player.m_targetEnemy = null;
        }

        rollCounter = 0.0f;
        inputActions["Look"].started -= OnLook;
        inputActions["Jump"].started -= OnRoll;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        if (!isTargetting && Mathf.Abs(context.ReadValue<Vector2>().x) > 1.0f)
        {
            FindAnotherTarget(context.ReadValue<Vector2>().x);
            isTargetting = true;
        }
    }

    private void OnRoll(InputAction.CallbackContext context)
    {
        if (!isRolling)
        {
            isRolling = true;
            PerformRoll();
        }
    }

    private void TargettingCount()
    {
        if (isTargetting)
        {
            targettingCounter += Time.deltaTime;
            if (targettingCounter > maxTargettingCount)
            {
                targettingCounter = 0.0f;
                isTargetting = false;
            }
        }
    }
}
