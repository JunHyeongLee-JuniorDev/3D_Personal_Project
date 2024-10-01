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

    public virtual void LateUpdate()
    {
        
    }

    public virtual void OnHurt()
    {
        if (monster.statData.currentHealth > 0)
        {
            animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.Hit], 0.1f);
            monster.hurtTimer.StartTimer(() => { });
        }

        else
        {
            animator.CrossFade(aniDB.monsterAniClips[EMonsterAni.Death], 0.1f);
            monster.TurnOffNav();
            monster.isDead = true;
            monster.activefalseForDeath();
        }
    }

    public virtual void PhysicsUpdate()
    {
        
    }

    public virtual void Update()
    {

    }
}
