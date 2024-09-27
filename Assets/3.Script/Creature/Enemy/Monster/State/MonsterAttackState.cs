using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAttackState : MonsterBaseState
{
    private Transform player;
    private int[] attackClips;
    private int[] skillClips;
    private int attackIndex;
    private float attackDistance;

    private bool isSkill;
    public MonsterAttackState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
        switch (monsterSO.MonsterType)
        {
            case EMonsterType.None:
                Debug.Log("�������� ���� Ÿ��");
                break;

            case EMonsterType.Minotaur:
                attackClips = aniDB.minoAttackClips;
                skillClips = null;
                break;

            case EMonsterType.Head:
                attackClips = aniDB.headAttackClips;
                skillClips = aniDB.headSkillClips;
                break;

            case EMonsterType.Oak:
                Debug.Log("Oak ���� Ŭ��");
                break;

            case EMonsterType.Skelleton:
                Debug.Log("Wizard ���� Ŭ��");
                break;
        }
    }

    public override void Enter()
    {
        Debug.Log($"{monsterSO.name} Attack State");
        base.Enter();
        player = monster.player;
        RandomAttackState();
        navAI.stoppingDistance = attackDistance;
        monster.TurnOnNav();
        animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.RunBlend], 0.2f);
        animator.SetFloat(aniDB.monsterParams[EMonsterAni.RunBlend], 1.0f);
    }

    public override void Update()
    {
        base.Update();

        navAI.SetDestination(player.position);

        if (!monster.attackTimer.isTickin)
        {
            CheckAttackDistance();
            monster.CheckPlayerDistance();
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.TurnOnNav();
        monster.attackTimer.StopTimer();
    }

    private void RandomAttackState()
    {
        if (skillClips != null)
        {
            if (Random.Range(0, 1) == 5)
            {
                attackIndex = Random.Range(0, skillClips.Length);
                Debug.Log("��ų ���õ� : " + skillClips[attackIndex]);
                isSkill = true;
                attackDistance = monsterSO.MonsterSkills[attackIndex].AttackDistance;
            }

            else
            {
                attackIndex = Random.Range(0, attackClips.Length);
                Debug.Log("�⺻ ���� ���õ�");
                isSkill = false;
                attackDistance = monsterSO.NormalAttacks[attackIndex].AttackDistance;
            }
        }

        else
        {
            attackIndex = Random.Range(0, attackClips.Length);
            Debug.Log("�⺻ ���� ���õ�");
            isSkill = false;
            attackDistance = monsterSO.NormalAttacks[attackIndex].AttackDistance;
        }
    }

    private void Attack()
    {
        monster.TurnOffNav();
        monster.attackTimer.StartTimer(() =>
        {
            monster.isAttack = false;
            monster.TurnOnNav();
        });
    }

    private void UseSkill()
    {
        animator.CrossFade(skillClips[attackIndex], 0.2f);
        monster.attackTimer.UpdateMaxTime(monsterSO.MonsterSkills[attackIndex].AttackAniTime);
        Attack();
    }

    private void UseNormalAttack()
    {
        animator.CrossFade(attackClips[attackIndex], 0.2f);
        monster.attackTimer.UpdateMaxTime(monsterSO.NormalAttacks[attackIndex].AttackAniTime);
        Attack();
    }


    private void CheckAttackDistance()
    {
        if (!navAI.pathPending && navAI.remainingDistance > monsterSO.RotateDistance)
            monster.isAttack = false;

        else if (!navAI.pathPending && navAI.remainingDistance <= navAI.stoppingDistance)
        {
            if(isSkill)
                UseSkill();

            else
                UseNormalAttack();
        }
    }
}
