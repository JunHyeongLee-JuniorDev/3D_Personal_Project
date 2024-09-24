using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLocoState : MonsterBaseState
{
    public MonsterLocoState(MonsterStateMachine stateMachine) : base(stateMachine) {}

    /*
    * 노드를 계속 따라가도록하자
    * 노드는 배열로 들고 있어 랜덤 값으로 index접근
    * 노드 까는 것은 인스펙터에서 직접할당하자
    */



    public override void Enter()
    {
        base.Enter();
    }
}
