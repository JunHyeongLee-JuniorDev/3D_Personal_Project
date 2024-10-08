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
                Debug.Log("정해지지 않은 타입");
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
                Debug.Log("Oak 없는 클립");
                break;

            case EMonsterType.Crab:
                attackClips = aniDB.crabAttackClips;
                skillClips = aniDB.crabSkillClips;
                break;
        }
    }

    public override void Enter()
    {
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
        if (player == null)
        {
            monster.isAttack = false;
            return;
        }

        navAI.SetDestination(player.position);

        if (!monster.attackTimer.isTickin)
        {
            CheckAttackDistance();
            monster.CheckPlayerDistance();
        }
        Vector3 targetPos = player.position;
        targetPos.y = monster.transform.position.y;

        monster.transform.rotation = Quaternion.LookRotation(Vector3.Lerp(monster.transform.forward,
                                (targetPos - monster.transform.position).normalized, monster._rotLerp));
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
            if (Random.Range(0, 5) == 0)
            {
                attackIndex = Random.Range(0, skillClips.Length);
                isSkill = true;
                attackDistance = monsterSO.MonsterSkill.AttackDistance;
            }

            else
            {
                attackIndex = Random.Range(0, attackClips.Length);
                isSkill = false;
                attackDistance = monsterSO.NormalAttacks[attackIndex].AttackDistance;
            }
        }

        else
        {
            attackIndex = Random.Range(0, attackClips.Length);
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
        monster.OnSkill?.Invoke();
        monster.attackTimer.UpdateMaxTime(monsterSO.MonsterSkill.AttackAniTime);
        if (monsterSO.MonsterType == EMonsterType.Head)
            Managers.Instance.Sound.Play3DSound("Monster/BookHeadSkill", monster.transform.position);

        Attack();
    }

    private void UseNormalAttack()
    {
        monster.weaponTrigger.UpdateDamage(monsterSO.NormalAttacks[attackIndex].Damage);
        animator.CrossFade(attackClips[attackIndex], 0.2f);
        monster.attackTimer.UpdateMaxTime(monsterSO.NormalAttacks[attackIndex].AttackAniTime);
        Attack();
    }


    private void CheckAttackDistance()
    {
        if (!navAI.pathPending && navAI.remainingDistance > monsterSO.RotateDistance || player == null)
            monster.isAttack = false;

        else if (!navAI.pathPending && navAI.remainingDistance <= navAI.stoppingDistance)
        {
            if(isSkill)
                UseSkill();

            else
                UseNormalAttack();
        }
    }

    public override void OnHurt()
    {
        if (monster.statData.currentHealth > 0)
        {
            animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.Hit], 0.01f);
            monster.hurtTimer.StartTimer(() =>
            {
                monster.isAttack = false;
            });
        }

        else
        {
            monster.isDead = true;
        }
    }
}
