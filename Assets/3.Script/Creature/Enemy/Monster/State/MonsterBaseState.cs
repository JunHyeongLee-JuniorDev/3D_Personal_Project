using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBaseState : IState
{
    protected MonsterSO monsterSO;
    protected MonsterController monster;
    protected MonsterAniDataBase aniDB;
    protected Animator animator;
    protected NavMeshAgent navAI;
    public MonsterBaseState(MonsterStateMachine stateMachine)
    {
        this.monster = stateMachine.monster;
        monsterSO = monster.monsterSO;
        aniDB = monster.aniDataBase;
        animator = monster.animator;
        navAI = monster.navAI;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {
        
    }

    public virtual void Update()
    {

    }

    public virtual void LateUpdate()
    {
        
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void OnHurt()
    {
        monster.hurtTimer.StopTimer();
        monster.weaponControl.TurnOffHitCol();

        if (monster.statData.currentHealth > 0)
        {
            animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.Hit], 0.01f);
            monster.hurtTimer.StartTimer(() => 
            {
                if (monster.isInRotateRad)
                    animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.RotateBlend], 0.25f);

                else
                    animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.RunBlend], 0.25f);
            });
        }

        else
        {
            monster.isDead = true;
        }
    }

    public virtual void Clear()
    {

    }
}
