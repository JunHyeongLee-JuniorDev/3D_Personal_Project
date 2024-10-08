using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BossLocoState : BossBaseState
{
    private float locoAniBlend = 0.0f;

    public BossLocoState(MonsterStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        locoAniBlend = 0.0f;
        monster.TurnOnNav();

        if (Random.Range(0, 10) == 0)
        {
            monster.isChargeState = true;
        }

        else
        {
            navAI.speed = monster.walkSpeed;
            navAI.stoppingDistance = monster.ChaseStopDistance;
            SetWalk();
        }
    }


    public override void Update()
    {
        if (monster.player == null) { monster.CancelAllConditions(); return; }
        base.Update();
        navAI.SetDestination(monster.player.position);
        if(!monster.isChargeState) monster.isInRotateRad = monster.CheckPlayerDistance();
        AnimationBlend();
    }

    private void AnimationBlend()
    {
        locoAniBlend = Mathf.Lerp(locoAniBlend, navAI.velocity.sqrMagnitude, 0.01f);
        animator.SetFloat(aniDB.aniParam[EPriestAni.WalkBlend], locoAniBlend);
    }

    private void SetWalk()
    {
        monster.TurnOnNav();
        animator.CrossFade(aniDB.aniHashs[EPriestAni.WalkBlend], 0.2f);
    }
}

public class BossRestState : BossBaseState
{
    public BossRestState(MonsterStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        navAI.updatePosition = false;
        navAI.updateRotation = false;
        monster.hpSlider.gameObject.SetActive(false);
    }

    public override void Update()
    {
        if (monster.player == null) { monster.CancelAllConditions(); return; }

        base.Update();
        navAI.SetDestination(monster.player.position);

        if (!navAI.pathPending && !(navAI.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial))
        {
            monster.TurnOnNav();
            monster.isPlayerEnter = true;
        }
    }

    public override void Exit()
    {
        monster.hpSlider.gameObject.SetActive(true);
        Managers.Instance.Sound.Play("Bgm/Boss", SoundManager.Sound.Bgm);
        base.Exit();
    }
}