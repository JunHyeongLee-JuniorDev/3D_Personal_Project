using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossBaseState : IState
{
    protected MonsterSO monsterSO;
    protected BossController monster;
    protected PriestAniDB aniDB;
    protected Animator animator;
    protected NavMeshAgent navAI;
    public BossBaseState(MonsterStateMachine stateMachine)
    {
        this.monster = stateMachine.monster as BossController;
        monsterSO = monster.monsterSO;
        aniDB = monster.aniDB;
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
        if (monster.currentHealth > 0)
        {
            animator.CrossFade(aniDB.aniHashs[EPriestAni.Groggy], 0.1f);
            monster.hurtTimer.StartTimer(() => { });
        }

        else
        {
            animator.CrossFade(aniDB.aniHashs[EPriestAni.Death], 0.1f);
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
