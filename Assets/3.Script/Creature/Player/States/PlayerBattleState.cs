using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBattleState : PlayerBaseState
{
    private float battleAniX;
    private float battleAniY;
    private Collider targetEnemy;
    private Collider nearlistEnemy;
    public PlayerBattleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        targetEnemy = null;
        nearlistEnemy = null;
        battleAniX = 0.0f;
        battleAniY = 0.0f;
        player.m_Controller.Move(Vector3.zero);
        animator.CrossFade(DTAniClipID[EPlayerAni.BATTLE], 0.2f);
        AssignTarget(true, true);
    }

    public override void Update()
    {
        if (targetEnemy == null) player.isBattle = false;

        if (Input.GetKeyDown(KeyCode.T))
            player.isBattle = false;

        base.Update();
        animator.SetFloat(DTAniParamID[EPlayerAniParam.BATTLEX], battleAniX);
        animator.SetFloat(DTAniParamID[EPlayerAniParam.BATTLEY], battleAniY);
        Move();
        Gravity();
        findAnotherTarget();

        Debug.DrawRay(player.m_mainCam.transform.position, dirBetAB(player.m_mainCam.transform.position, targetEnemy.transform.position), Color.red);
    }
    public override void Move()
    {
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
    private void LookAtTarget()//짐승의 똥새끼!!!
    {
        player.lockOnCanvas.transform.position = targetEnemy.transform.position;
        player.lockOnCanvas.transform.localScale = Vector3.one * (player.m_mainCam.transform.position - targetEnemy.transform.position).magnitude * player.lockOnCanvasScale;

        Quaternion targetAngle = Quaternion.LookRotation(targetEnemy.transform.position - player.transform.position);

        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetAngle, Time.deltaTime * groundData.lookTargetSmoothTime);
    }
    protected override void Gravity()
    {
        if (player.isGrouded) { player.m_verticalVelocity = -2f; return; }

        else
        {
            player.isFall = true;
            player.isBattle = false;
            player.isJump = false;
        }

        base.Gravity();
    }
    private void AssignTarget(bool isInit, bool isRight)
    {
        Collider[] enemys = Physics.OverlapSphere(player.transform.position, player.radiusOfView, player.enemyLayer, QueryTriggerInteraction.Ignore);
        if (enemys == null) return;

        float nearlistEnemyDist = -1.0f;
        Vector3 _direction;
        Vector3 _enemyPos;
        float _distanceCamAndEnemy;
        nearlistEnemy = null;

        foreach (Collider enemy in enemys)
        {
            _enemyPos = enemy.transform.position;

            if (isInCam(_enemyPos, out Vector3 _enemyViewPos))
            {
                if (!isInit)
                {
                    if(enemy.Equals(targetEnemy) || 
                      (isRight && (_enemyViewPos.x < 0.5f)) ||
                      (!isRight && (_enemyViewPos.x > 0.5f))) continue;
                }

                _direction = dirBetAB(player.transform.position, _enemyPos).normalized;
                _distanceCamAndEnemy = Vector3.Distance(player.transform.position, _enemyPos);

                if (Physics.Raycast(player.m_mainCam.transform.position, _direction, out RaycastHit hitInfo,
                                _distanceCamAndEnemy, player.wallAndGroundLayer))
                    continue;

                else
                {
                    if (_distanceCamAndEnemy < nearlistEnemyDist ||
                        nearlistEnemyDist < 0.0f)
                    {
                        nearlistEnemyDist = _distanceCamAndEnemy;
                        nearlistEnemy = enemy;
                    }
                }
            }

            if (nearlistEnemy != null) targetEnemy = nearlistEnemy;
        }


    }
    private void findAnotherTarget()
    {
        if (player.m_input.look.Equals(Vector2.zero)) return;

        if (player.m_input.look.x > 1.0f)
            AssignTarget(false, true);

        if(player.m_input.look.x < -1.0f)
            AssignTarget(false, false);

        Debug.Log("다른 Enemy를 찾는 중");
    }
    private Vector3 dirBetAB(Vector3 origin, Vector3 target)
    {
        return (target - origin);
    }
    private bool isInCam(Vector3 position, out Vector3 viewPos)
    {
        viewPos = player.m_mainCam.WorldToViewportPoint(position);

        if (viewPos.x >= 0 && viewPos.x <= 1
         && viewPos.y >= 0 && viewPos.y <= 1
         && viewPos.z > 0)
            return true;

        else return false;
    }
    public override void Exit()
    {
        base.Exit();
        player.isBattle = false;
    }
}
