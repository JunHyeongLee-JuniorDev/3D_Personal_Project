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
        monster.UpdateGroggy(10.0f);

        if(monster.statData.currentHealth <= 0.0f)
        {
            animator.CrossFade(aniDB.aniHashs[EPriestAni.Death], 0.1f);
            monster.TurnOffAllBossAttackCol();
            monster.TurnOffNav();
            monster.isDead = true;
            monster.statData.isDead = true;
            monster.activefalseForDeath();
            Managers.Instance.Game.OnKillBoss();
            Managers.Instance.Data.SavePlayerPosition();
            Managers.Instance.Data.SaveGame(Managers.Instance.Data.currentSaveIndex);
            monster.enabled = false;
        }

        else if (monster.currentGroggy >= monster.maxGroggy && !monster.hurtTimer.isTickin)
        {
            animator.CrossFade(aniDB.aniHashs[EPriestAni.Groggy], 0.1f);
            monster.currentGroggy = 0.0f;

            monster.TurnOffNav();
            monster.TurnOffAllBossAttackCol();
            monster.TurnOffBossAttackCol();

            monster.hurtTimer.StartTimer(() =>
            {
                if (monster.isInRotateRad)
                    animator.CrossFade(aniDB.aniHashs[EPriestAni.RotateBlend], 0.1f);
                else
                    animator.CrossFade(aniDB.aniHashs[EPriestAni.WalkBlend], 0.1f);

                monster.TurnOnNav();
            });
        }
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Clear()
    {

    }
}
