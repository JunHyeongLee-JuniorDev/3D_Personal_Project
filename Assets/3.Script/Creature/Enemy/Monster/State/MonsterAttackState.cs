using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAttackState : MonsterBaseState
{
    private Transform player;
    private int[] attackClips;
    private int attackIndex;

    public MonsterAttackState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
        switch (monsterSO.MonsterType)
        {
            case EMonsterType.None:
                Debug.Log("정해지지 않은 타입");
                break;

            case EMonsterType.Minotaur:
                attackClips = aniDB.minoAttackClips;
                break;

            case EMonsterType.Wizard:
                attackClips = aniDB.NecAttackClips;
                break;

            case EMonsterType.Oak:
                Debug.Log("Oak 없는 클립");
                break;

            case EMonsterType.Skelleton:
                Debug.Log("Wizard 없는 클립");
                break;
        }
    }

    public override void Enter()
    {
        Debug.Log($"{monsterSO.name} Attack State");
        base.Enter();
        player = monster.player;
        navAI.SetDestination(player.transform.position);
        navAI.isStopped = false;
        navAI.stoppingDistance = monster.attackDistance;
        animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.RunBlend], 0.2f);
        animator.SetFloat(aniDB.monsterParams[EMonsterAni.RunBlend], 1.0f);
        attackIndex = 0;
    }

    public override void Update()
    {
        base.Update();

        navAI.SetDestination(player.position);
        if(monster.CheckPlayerDistance() && !monster.attackTimer.isTickin)
            RandomAttackState();

        if (!monster.attackTimer.isTickin)
            CheckAttackDistance();
    }

    public override void Exit()
    {
        base.Exit();
        navAI.isStopped = false;
        navAI.updatePosition = true;
        navAI.updateRotation = true;
        monster.attackTimer.StopTimer();
    }

    private void RandomAttackState()
    {
        if (Random.Range(0,5) == 0)
        {
            int newIndex = Random.Range(0, attackClips.Length);

            if (newIndex == attackIndex) newIndex = newIndex == 0 ? attackClips.Length - 1 : newIndex - 1;
            attackIndex = newIndex;
            navAI.stoppingDistance = monsterSO.MonsterSkills[attackIndex].AttackDistance;
            Attack();
        }

        else
        {
            Debug.Log("공격인데 일부러 나감");
            monster.isAttack = false;
        }
    }

    private void Attack()
    {
        animator.CrossFade(attackClips[attackIndex], 0.2f);
        monster.attackTimer.UpdateMaxTime(monsterSO.MonsterSkills[attackIndex].AttackAniTime);
        Debug.Log("공격 애니 시작");
        monster.TurnOffNav();
        monster.attackTimer.StartTimer(() => 
        {
            Debug.Log("공격 끝남");
            monster.TurnOnNav();
            animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.RunBlend], 0.2f);
            CheckAttackDistance();
        });
    }

    private void CheckAttackDistance()
    {
        if (!navAI.pathPending &&
            Vector3.Distance(monster.transform.position, player.position)
            > monsterSO.RotateDistance)
            monster.isAttack = false;
    }
}
