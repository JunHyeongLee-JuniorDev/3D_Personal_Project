using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterStateMachine : StateMachine
{
    public MonsterController monster;

    public MonsterStateMachine(MonsterController monster)
    {
        this.monster = monster;
    }
}
