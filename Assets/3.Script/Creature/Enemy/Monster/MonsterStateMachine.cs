using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : StateMachine
{
    public MonsterController monster;

    public MonsterStateMachine(MonsterController monster)
    {
        this.monster = monster;
    }

}
