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
         * ������ ����(float)�� ��ȯ�Ѵ�. 
         * �̴� �� �������� �Ÿ��� ���ϰų� �ӷ��� ���� �� ����� �� �ִ�.
         */

        Vector2 _input = player.m_input.move;

        float _speedOffset = 0.1f;

        //��ǥ �ӵ����� ����
        if (player.m_speed < player.m_targetSpeed - _speedOffset ||
            player.m_speed > player.m_targetSpeed + _speedOffset)
        {
            player.m_speed = Mathf.Lerp(player.m_speed, player.m_targetSpeed, Time.deltaTime * groundData.speedChangeRate);

            // �Ҽ��� 3° �ڸ������� ����ϱ�
            // ���ʿ��� ���� ���̱Ⱑ ����
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
        player.lockOnCanvas.transform.position = player.m_targetEnemy.transform.position; // ĵ������ ��ġ�� Ÿ�� ��ġ�� �ٲߴϴ�.
        player.lockOnTarget.position = player.m_targetEnemy.transform.position;// ī�޶��� look at�� Ÿ�� ��ġ�� �ű�ϴ�.
        player.lockOnCanvas.transform.localScale = Vector3.one * 
                                                  (dirToTarget).magnitude * 
                                                   player.lockOnCanvasScale; // �Ÿ��� ����Ͽ� Ÿ���� UI�� ũ�⸦ Ű��ϴ�.

        Vector3 targetPos = new Vector3(player.m_targetEnemy.transform.position.x, player.transform.position.y, player.m_targetEnemy.transform.position.z);
        //Ÿ���� y Position�� �÷��̾��� y�����ǰ� �ٸ��⿡ �����ݴϴ�.
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
    /// ���� ������ OverlapShpere�� ī�޶��� �þ߰� ���� ��ü�� Ÿ�����Ͽ� PlayerController�� �����մϴ�.
    /// </summary>
    /// <param name="isInit">�ʱ�ȭ �� �þ߰��� ���� ������ ��� ���� ȭ�� �߾ӿ� ���� �� ��ü�� ��ǥ�� �մϴ�.</param>
    /// <param name="isRight">�ʱ�ȭ�� �ƴ� �� ȭ���� ������ ��ü Ȥ�� ���ʿ� �ִ� ��ü�� ����ϴ�.</param>
    private void AssignTarget(bool isInit, bool isRight)
    {
        Collider[] enemys = Physics.OverlapSphere(player.transform.position, player.radiusOfView, player.enemyLayer, QueryTriggerInteraction.Ignore);
        // ���� �ȿ� ���� �ִٸ� �迭�� ����ϴ�.

        if (enemys == null ||
            isInit && player.m_targetEnemy != null) return;

        float nearlistEnemyDist = -1.0f; // ������ �Ÿ��� ����̹Ƿ� ó������ ������ �������ݴϴ�.

        // For Cashing
        Vector3 _direction;
        Vector3 _enemyPos;
        float _distancePlayerAndEnemy;
        nearlistEnemy = null;
        // For Cashing

        foreach (Collider enemy in enemys)
        {
            _enemyPos = enemy.transform.position;

            if (isInCam(_enemyPos, out Vector3 _enemyViewPos)) // ���� ī�޶� ���� �ִٸ�
            {
                if (!isInit)
                {
                    if(enemy.Equals(player.m_targetEnemy) || 
                      (isRight && (_enemyViewPos.x < 0.5f)) ||
                      (!isRight && (_enemyViewPos.x > 0.5f))) continue;
                    // �÷��̾ ������ Ȥ�� �������� Ž���� ��û ���� ���� ���̶�� continue
                }

                _direction = (_enemyPos - player.transform.position); // ���� �÷��̾��� ���� ����
                _distancePlayerAndEnemy = Vector3.Distance(player.transform.position, _enemyPos); // ���� �÷��̾��� �Ÿ�

                if (!Physics.Raycast(player.m_mainCam.transform.position, _direction.normalized, out RaycastHit hitInfo,
                                _distancePlayerAndEnemy, player.wallAndGroundLayer))// �Ÿ����� ray�� ���� ��ֹ� �˻�
                {
                    if (_distancePlayerAndEnemy < nearlistEnemyDist ||
                        nearlistEnemyDist < 0.0f) // ���� ����� ���̶�� ���ο� ����� ������ ����
                    {
                        nearlistEnemyDist = _distancePlayerAndEnemy;
                        nearlistEnemy = enemy;
                    }
                }
            }
        }

        if (nearlistEnemy != null)
        {
            if (player.m_targetEnemy != null) // ������� ���� ���� ��ü üũ
                player.m_targetEnemy.GetComponent<Renderer>().material.color = Color.yellow;
            player.m_targetEnemy = nearlistEnemy;
            player.m_targetEnemy.GetComponent<Renderer>().material.color = Color.red;// ������� ���� ���� ��ü üũ
        }

        else // �� ������ �����ϴ� ��ü�� ���ٸ� battle state ����
        {
            if (isInit)
                player.isBattle = false;
        }
    }



    /// <summary>
    /// ���� ���콺�� x delta ���� 1.0 �̻��̸� Ÿ�� ����(InputAction���� ȣ��)
    /// </summary>
    /// <param name="x">���콺 x�� delta ��</param>
    private void FindAnotherTarget(float x)
    {
        if (x.Equals(0.0f)) return; // Ȥ�� �� ���� ó��

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
    /// ��ü�� ȭ�� ���� �ִٸ� true�� ��ȯ
    /// </summary>
    /// <param name="position"> ��ü�� position</param>
    /// <param name="viewPos"> ViewPort�� ��ȯ�� ��ü ����� ���ؼ� out(����)</param>
    /// <returns></returns>
    private bool isInCam(Vector3 position, out Vector3 viewPos)
    {
        viewPos = player.m_mainCam.WorldToViewportPoint(position);
        /*ī�޶��� Viewport�� 0~1�� ����ȭ �Ǿ��ִ�. ���� �ϴ�(0, 0), ���� ���(1, 1) z�� ī�޶�κ����� ��ü�� world position�̴�*/

        if (viewPos.x >= 0 && viewPos.x <= 1
         && viewPos.y >= 0 && viewPos.y <= 1
         && viewPos.z > 0)
            return true;

        else return false;
    }

    private bool isItSoFar()
    {
        if (Vector3.Distance(player.transform.position, player.m_targetEnemy.transform.position) > player.radiusOfView) // ��ü ���� ���� ���̶�� ���
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
    /// ��� Ÿ���� ��� ����(�̱��� ���� : �� ��ü �����, ���� �� �ڿ� �ִٸ�)
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
            Debug.Log("���� ������ ��");
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
