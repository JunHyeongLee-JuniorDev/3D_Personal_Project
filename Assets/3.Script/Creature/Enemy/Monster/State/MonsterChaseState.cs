using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterChaseState : MonsterBaseState
{
    private Transform player;
    private float runAniBlend;

    public MonsterChaseState(MonsterStateMachine stateMachine) : base(stateMachine){}


    public override void Enter()
    {
        base.Enter();

        player = monster.player;
        navAI.stoppingDistance = monster.ChaseStopDistance;
        runAniBlend = 0f;
        navAI.speed = monsterSO.RunSpeed;

        animator.CrossFade(monster.aniDataBase.monsterAniClips[EMonsterAni.RunBlend], 0.25f);
    }

    public override void Update()
    {
        base.Update();
        navAI.SetDestination(player.position);
        CheckPlayerDistance();
        AnimationBlend();
    }

    private void AnimationBlend()
    {
        runAniBlend = Mathf.Lerp(runAniBlend, navAI.desiredVelocity.magnitude, 0.1f);
        animator.SetFloat(aniDB.monsterParams[EMonsterAni.RunBlend], runAniBlend);
    }

    public void CheckPlayerDistance()
    {
        monster.isInRotateRad = !navAI.pathPending &&
                                     navAI.remainingDistance <= navAI.stoppingDistance;
    }
}
