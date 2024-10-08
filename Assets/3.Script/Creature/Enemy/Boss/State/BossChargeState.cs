using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargeState : BossBaseState
{
    public BossChargeState(MonsterStateMachine stateMachine) : base(stateMachine) {}

    private readonly int wallHitMaxCount = 3;

    private Vector3 wallPos;
    private Coroutine chargeCash;

    public override void Enter()
    {
        base.Enter();
        monster.TurnOnNav();
        navAI.speed = monster.chargeSpeed;
        navAI.stoppingDistance = monster.PatrolStopDistance;
        animator.CrossFade(aniDB.aniHashs[EPriestAni.Block], 0.2f);
        animator.SetLayerWeight(1, 1.0f);
        RunToPlayer();
    }

    private void RunToPlayer()
    {
        if (chargeCash != null) monster.StopCoroutine(chargeCash);

        chargeCash = monster.StartCoroutine(wallCharge_co());
    }

    private IEnumerator wallCharge_co()
    {
        for (int i = 0; i < wallHitMaxCount; i++)
        {
            Physics.Raycast(monster.transform.position + new Vector3(0.0f, 2.0f, 0.0f), (monster.player.position - monster.transform.position).normalized, out RaycastHit hitInfo, 200f, monster.wallLayer);
            //플레이어 방향 벡터로 ray 쏘고

            animator.CrossFade(aniDB.aniHashs[EPriestAni.AttackRun], 0.2f);
            // 그 방향에 맞은 벽에 돌진

            float turnOnWallCheckerTime = 1.0f;

            while (true)
            {
                if (monster.player == null) { monster.CancelAllConditions(); yield break; }
                if (turnOnWallCheckerTime < 0.0f && monster.IsHitWall())
                {
                    Managers.Instance.Sound.Play3DSound("Monster/BossRunCrash", monster.transform.position);
                    break;
                }
                turnOnWallCheckerTime -= Time.deltaTime;
                navAI.SetDestination(hitInfo.point);
                yield return null;
            }
            monster.wallCrashEFF.Play(true);
        }
        animator.SetLayerWeight(1, 0.0f);
        animator.CrossFade(aniDB.aniHashs[EPriestAni.Groggy], 0.2f);
        monster.TurnOffAllBossAttackCol();

        monster.hurtTimer.StartTimer(() =>
        {
            monster.isChargeState = false;
        });
    }

    public override void Exit()
    {
        base.Exit();
        monster.TurnOffAllBossAttackCol();
    }
}
