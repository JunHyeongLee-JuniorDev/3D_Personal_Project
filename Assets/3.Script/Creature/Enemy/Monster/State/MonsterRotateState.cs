using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using Unity.IO.LowLevel.Unsafe;
using System.Runtime.InteropServices.WindowsRuntime;

public class MonsterRotateState : MonsterBaseState
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
    public MonsterRotateState(MonsterStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        Debug.Log("Monster Rotate State");
        base.Enter();
        player = monster.player;

        navAI.isStopped = true;
        navAI.velocity = Vector3.zero;
        InitBlend();
        if (coroutineCash != null ) monster.StopCoroutine(coroutineCash);
        coroutineCash = monster.StartCoroutine(SwtichBehave());

        animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.RotateBlend], 0.25f);
    }

    public override void Update()
    {
        if (monster.hurtTimer.isTickin) return;
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

            else
            {
                monster.isAttack = true;
                yield break;
            }
        }
    }

    private void MoveAroundPlayer()
    {
        if (distanceWithPlayer < 3.0f && targetY > 0.0f)
            targetY = -1.0f;

        blendX = Mathf.Lerp(blendX, targetX, 0.01f);
        blendY = Mathf.Lerp(blendY, targetY, 0.01f);
        animator.SetFloat(aniDB.monsterParams[EMonsterAni.BlendX], blendX);
        animator.SetFloat(aniDB.monsterParams[EMonsterAni.BlendY], blendY);

        Vector3 targetPos = player.position;
        targetPos.y = monster.transform.position.y;

        monster.transform.rotation = Quaternion.LookRotation(Vector3.Lerp(monster.transform.forward, 
                                (targetPos - monster.transform.position).normalized, monster._rotLerp));
        monster.transform.Translate(blendY * Vector3.forward * monsterSO.PatrolSpeed * Time.deltaTime * 0.5f);
        monster.transform.RotateAround(player.position,Vector3.down * blendX , monsterSO.RotateAroundSpeed * Time.deltaTime);
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
        if(player == null) return false;
        distanceWithPlayer = Vector3.Distance(monster.transform.position, player.position);
        return distanceWithPlayer < monster.ChaseStopDistance + 1f;
    }

    public override void OnHurt()
    {
        monster.hurtTimer.StopTimer();
        monster.weaponControl.TurnOffHitCol();

        if (monster.statData.currentHealth > 0)
        {
            animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.Hit], 0.01f);
            monster.hurtTimer.StartTimer(() =>
            {
                animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.RotateBlend], 0.2f);
            });
        }

        else
        {
            if (coroutineCash != null) monster.StopCoroutine(coroutineCash);
            monster.isDead = true;
        }
    }
}

public class MonsterDeathState : MonsterBaseState
{
    public MonsterDeathState(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Monster Death State");
        animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.Death], 0.1f);
        monster.TurnOffNav();
        monster.activefalseForDeath();

        switch (monsterSO.MonsterType)
        {
            case EMonsterType.Minotaur:
                Managers.Instance.Sound.Play3DSound("Monster/MinoDead", monster.transform.position);
                break;
            case EMonsterType.Head:
                Managers.Instance.Sound.Play3DSound("Monster/BookHeadSkill", monster.transform.position);
                break;
        }

        if (monster.statData.isDead)
        {
            Managers.Instance.Sound.ClearSound();
            Managers.Instance.Sound.Play("Bgm/Game");
        }
    }
}