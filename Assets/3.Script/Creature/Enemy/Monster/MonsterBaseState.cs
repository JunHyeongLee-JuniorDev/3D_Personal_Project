using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBaseState : IState
{
    private MonsterController monster;
    private MonsterAniDataBase MonsterAniDataBase;

    public MonsterBaseState(MonsterStateMachine stateMachine)
    {
        this.monster = stateMachine.monster;
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
