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
    /// �÷��̾�� Ÿ���� �ٶ󺸱⿡ �ܼ��ϰ� 8�������θ� �����̴� ����, �ִϸ��̼� ���� �� ����
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

        if(player.m_Controller.enabled)
        player.m_Controller.Move((movement3D * player.m_speed + new Vector3(0.0f, player.m_verticalVelocity, 0.0f)) * Time.deltaTime);
    }
    /// <summary>
    /// Ÿ���� UI, ī�޶� Ÿ������ Ÿ�ٿ� ��ġ�ϰ� �÷��̾�� Lerp�� ����Ͽ� Ÿ���� �ٶ󺾴ϴ�.
    /// </summary>
    private void LookAtTarget()
    {
        player.lockOnUI.transform.position = player.m_targetEnemy.transform.position; // ĵ������ ��ġ�� Ÿ�� ��ġ�� �ٲߴϴ�.
        player.lockOnTarget.position = player.m_targetEnemy.transform.position;// ī�޶��� look at�� Ÿ�� ��ġ�� �ű�ϴ�.
        player.lockOnUI.transform.localScale = Vector3.one * 
                                                  (dirToTarget).magnitude * 
                                                   player.lockOnUIScale; // �Ÿ��� ����Ͽ� Ÿ���� UI�� ũ�⸦ Ű��ϴ�.

        //Ÿ���� y Position�� �÷��̾��� y�����ǰ� �ٸ��⿡ �����ݴϴ�.

        Vector3 targetVec = dirToTarget;
        targetVec.y = 0.0f;

        Quaternion targetAngle = Quaternion.LookRotation(targetVec);

        if(!player.rollBtnTimer.isTickin)
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, targetAngle, player.lookTargetRotLerp * Time.deltaTime);
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

        if (enemys.Length == 0 ||
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
            player.m_targetEnemy = nearlistEnemy;

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
    /// <summary>
    /// player.radiusOfView ������ ������ ���� ���� �ۿ� ������ Ÿ���� ���
    /// </summary>
    /// <returns></returns>
    private bool isItSoFar()
    {
        if (Vector3.Distance(player.transform.position, player.m_targetEnemy.transform.position) > player.radiusOfView) // ��ü ���� ���� ���̶�� ���
            return true;

        else return false;
    }
    /// <summary>
    /// Ÿ�� �������� raycast�� Update���� �߻��Ͽ� ��ֹ� ����
    /// </summary>
    /// <returns>��ֹ��̸� true, �ƴϸ� false</returns>
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
    /// ��� Ÿ���� ��� ����(�̱��� ���� : �� ��ü �����)
    /// </summary>
    private void CancelConditions()
    {
        if (player.m_targetEnemy == null ||
            isItSoFar() ||
            isBehindTheBuild())
            player.isBattle = false;
    }

    /// <summary>
    /// ������ �Է� �������� ������ ���� ī�޶� roll ���� ī�޶�� ��� ����
    /// </summary>
    private void PerformRoll()
    {
        // atan2�� ����ϸ� ���� �� a���� �� b�� ���� ���� ���͸� ���� �� �ִ�(�� a ������) ���� �̸� ���ȿ��� ������ �ٲٰ� 
        // �÷��̾� ĳ���� Ȥ�� ī�޶��� ���� ������ ���� ���ͷ� ������ ����
        // Atan, Atan2�� tan�� ���Լ��� �غ� �뺯�� �˰� �ִٸ� �� <L�� �� �� �ִ�.
        float rollRotation = Mathf.Atan2(player.m_input.move.x, player.m_input.move.y) * Mathf.Rad2Deg + 
                                         player.m_mainCam.transform.eulerAngles.y;
        player.m_Controller.Move(Vector3.zero);
        player.transform.rotation = Quaternion.Euler(0.0f, rollRotation, 0.0f);
        animator.CrossFade(DTAniClipID[EPlayerAni.ROLL], 0.25f);
    }
    
    /// <summary>
    /// ���� ��ǥ�� �ִ� Ÿ���� ī�޶� root�� LateUpdate���� forward�� Ÿ�� �������� ����
    /// </summary>
    /// <param name="originDir"> ķ���� Ÿ�� ������ ���� ���� </param>
    private void PlaceTheCam(Vector3 originDir)
    {
        player.lockOnTargetRoot.transform.forward = Vector3.Lerp(player.lockOnTargetRoot.transform.forward,originDir.normalized, player.lookTargetRotLerp * Time.deltaTime);
        player.thirdPersonCam.m_cinemachineCamTarget.transform.forward = originDir.normalized;
    }

    /// <summary>
    /// �ٸ� ������Ʈ���� �ʿ��� ��ɵ� Ȱ��ȭ, target �ٽ� null ȭ
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
    /// Ÿ���� callback �޼ҵ�(���콺 "��, ��"�� �ߵ�)
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
    /// �����̽��� = ������, ������ Ÿ�̸ӵ� ���� ����
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
