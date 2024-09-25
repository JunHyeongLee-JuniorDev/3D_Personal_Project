using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBaseState : IState
{
    protected MonsterSO monsterSO;
    protected MonsterController monster;
    protected MonsterAniDataBase aniDB;
    public MonsterBaseState(MonsterStateMachine stateMachine)
    {
        this.monster = stateMachine.monster;
        monsterSO = monster.monsterSO;
        aniDB = monster.aniDataBase;
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
        
    }

    public virtual void PhysicsUpdate()
    {
        
    }

    public virtual void Update()
    {

    }
}
