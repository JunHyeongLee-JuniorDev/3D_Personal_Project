using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossRotateState : BossBaseState
{
    /*
     * rotate state
     * 플레이어 주위를 도는 State 앞으로 혹은 뒤로 4가지 경우의 수
     * 그러다가 공격도 하고
     * 
     */

    private enum Behaves
    {
        Left,
        Right,
        Forward,
        Length
    };

    private float blendX;
    private float blendY;
    private float targetX;
    private float targetY;
    private float distanceWithPlayer;

    private Transform player;
    private Coroutine coroutineCash;
    public BossRotateState(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        player = monster.player;

        navAI.isStopped = true;
        navAI.velocity = Vector3.zero;
        InitBlend();
        if (coroutineCash != null) monster.StopCoroutine(coroutineCash);
        coroutineCash = monster.StartCoroutine(SwtichBehave());

        animator.CrossFade(aniDB.aniHashs[EPriestAni.RotateBlend], 0.25f);
    }

    public override void Update()
    {
        if (monster.player == null) { monster.CancelAllConditions(); return; }
        base.Update();
        monster.isInRotateRad = CanelCondition();
        MoveAroundPlayer();
    }

    public override void Exit()
    {
        base.Exit();
        if (coroutineCash != null) monster.StopCoroutine(coroutineCash);
        navAI.isStopped = false;
    }


    private IEnumerator SwtichBehave()
    {
        while (true)
        {
            if (Random.Range(0, 2) == 0)
            {
                monster.isAttack = true;
                yield break;
            }

            else
            {
                switch (Random.Range((int)Behaves.Left, (int)Behaves.Length))
                {
                    case (int)Behaves.Left:
                        targetX = -1.0f;
                        targetY = 0.0f;
                        break;

                    case (int)Behaves.Right:
                        targetX = 1.0f;
                        targetY = 0.0f;
                        break;

                    case (int)Behaves.Forward:
                        targetX = 0.0f;
                        targetY = 1.0f;
                        break;

                    case (int)Behaves.Length:
                        Debug.LogWarning("행동 길이 오류 발생");
                        break;
                }

                yield return new WaitForSeconds(Random.Range(1.5f, 3.0f));
            }
        }
    }

    private void MoveAroundPlayer()
    {
        if (distanceWithPlayer < 3.0f && targetY > 0.0f)
            targetY = -1.0f;

        else if (distanceWithPlayer < 7.0f &&
                 distanceWithPlayer > 6.0f
            && targetY < 0.0f)
            targetY = 1.0f;

        blendX = Mathf.Lerp(blendX, targetX, 0.02f);
        blendY = Mathf.Lerp(blendY, targetY, 0.02f);
        animator.SetFloat(aniDB.aniParam[EPriestAni.BlendX], blendX);
        animator.SetFloat(aniDB.aniParam[EPriestAni.BlendY], blendY);
        monster.transform.rotation = Quaternion.LookRotation(Vector3.Lerp(monster.transform.forward,
                                (player.position - monster.transform.position).normalized, monster._rotLerp));
        monster.transform.Translate(blendY * Vector3.forward * monster.walkSpeed * Time.deltaTime);
        monster.transform.RotateAround(player.position, Vector3.down * blendX, monster.rotateSpeed * Time.deltaTime);
    }

    private void InitBlend()
    {
        blendX = 0.0f;
        blendY = 0.0f;
        targetX = 0.0f;
        targetY = 0.0f;
    }

    private bool CanelCondition()
    {
        distanceWithPlayer = Vector3.Distance(monster.transform.position, player.position);
        return distanceWithPlayer < monster.ChaseStopDistance + 1f && player != null;
    }
}
