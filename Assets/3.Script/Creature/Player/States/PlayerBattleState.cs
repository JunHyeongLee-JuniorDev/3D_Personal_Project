using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBattleState : PlayerBaseState
{
    private float battleAniX;
    private float battleAniY;
    private Collider nearlistEnemy;
    private ThirdPersonCam thirdPersonCam;
    public PlayerBattleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) 
    { thirdPersonCam = player.thirdPersonCam; }

    public override void Enter()
    {
        nearlistEnemy = null;
        battleAniX = 0.0f;
        battleAniY = 0.0f;
        player.m_Controller.Move(Vector3.zero);
        inputActions["Look"].started += OnLook;
        player.cinemachineAnimator.Play("TargetCamera");
        animator.CrossFade(DTAniClipID[EPlayerAni.BATTLE], 0.2f);
        AssignTarget(true, true);
        player.thirdPersonCam.enabled = false;
    }

    public override void Update()
    {
        if(player.m_targetEnemy == null)
            AssignTarget(true, true);

        if (Input.GetKeyDown(KeyCode.T))
            player.isBattle = false;

        base.Update();
        animator.SetFloat(DTAniParamID[EPlayerAniParam.BATTLEX], battleAniX);
        animator.SetFloat(DTAniParamID[EPlayerAniParam.BATTLEY], battleAniY);
        Move();
        Gravity();
        LimitDist();

        Debug.DrawRay(player.m_mainCam.transform.position, player.m_mainCam.transform.position - player.m_targetEnemy.transform.position, Color.red);
    }

    public override void PhysicsUpdate()
    {
        Vector3 orignPos = new Vector3(thirdPersonCam.m_cinemachineCamTarget.transform.localEulerAngles.x, 0.0f, thirdPersonCam.m_cinemachineCamTarget.transform.localEulerAngles.z);

        if (thirdPersonCam.m_cinemachineCamTarget.transform.localEulerAngles.y > 0.1f)
            thirdPersonCam.m_cinemachineCamTarget.transform.localEulerAngles = Vector3.Lerp(thirdPersonCam.m_cinemachineCamTarget.transform.localEulerAngles,
                                                                                            orignPos,
                                                                                            0.3f);

        else if (thirdPersonCam.m_cinemachineCamTarget.transform.localEulerAngles.y < -0.1f)
            thirdPersonCam.m_cinemachineCamTarget.transform.localEulerAngles = Vector3.Lerp(thirdPersonCam.m_cinemachineCamTarget.transform.localEulerAngles,
                                                                                            orignPos,
                                                                                            0.3f);

        else
            thirdPersonCam.m_cinemachineCamTarget.transform.localEulerAngles = orignPos;
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
    private void LookAtTarget()//������ �˻���!!!
    {
        player.lockOnCanvas.transform.position = player.m_targetEnemy.transform.position;
        player.lockOnTarget.position = player.m_targetEnemy.transform.position;
        player.lockOnCanvas.transform.localScale = Vector3.one * 
        (player.m_mainCam.transform.position - player.m_targetEnemy.transform.position).magnitude * 
        player.lockOnCanvasScale;

        Quaternion targetAngle = Quaternion.LookRotation(player.m_targetEnemy.transform.position - player.transform.position);

        player.transform.rotation = targetAngle;
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
        if (enemys == null ||
            isInit && player.m_targetEnemy != null) return;

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
                    if(enemy.Equals(player.m_targetEnemy) || 
                      (isRight && (_enemyViewPos.x < 0.5f)) ||
                      (!isRight && (_enemyViewPos.x > 0.5f))) continue;
                }

                _direction = (player.transform.position - _enemyPos).normalized;
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

            if (nearlistEnemy != null) player.m_targetEnemy = nearlistEnemy;

            else
                player.isBattle = false;
        }


    }
    private void findAnotherTarget()
    {
        if (player.m_input.look.Equals(Vector2.zero)) return;

        else if (player.m_input.look.x > 1.0f)
            AssignTarget(false, true);

        else if(player.m_input.look.x < -1.0f)
            AssignTarget(false, false);
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

    private void LimitDist()
    {
        if (Vector3.Distance(player.transform.position, player.m_targetEnemy.transform.position) > 10.0f)
            player.isBattle = false;
    }

    public override void Exit()
    {
        base.Exit();

        if (!player.isBattle)
        {
            player.cinemachineAnimator.Play("FollowCamera");
            player.thirdPersonCam.enabled = true;
        }

        inputActions["Look"].started -= OnLook;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        findAnotherTarget();
    }
}
