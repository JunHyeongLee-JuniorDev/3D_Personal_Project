using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.AI;

public class PriestController : MonsterController
{
    /*
     * 기존 State를 그대로 쓰되 patrol state는 빼기로 하자
     * 같은 surface에 있으면 target를 바로 인식하도록...
     */


    protected override void Start()
    {
        navAI = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        weaponTrigger = GetComponentInChildren<MonsterWeaponTrigger>();
        hitBox = GetComponent<Collider>();
        aniDataBase = new MonsterAniDataBase();
        aniDataBase.init();

        //Inits
        attackTimer = new Timer(0.0f, this);
        hurtTimer = new Timer(2.0f, this);
        deathTimer = new Timer(3.0f, this);
        //hpSlider = Instantiate(hpPrefabs);
        maxHealth = monsterSO.MaxHealth;
        currentHealth = maxHealth;
        navAI.updateRotation = false;

        //Init States
        stateMachine = new MonsterStateMachine(this);

        var _locoState = new BossLocoState(stateMachine);
        var _runState = new BossRunState(stateMachine);
        var _rotateState = new BossRotateState(stateMachine);
        var _attackState = new BossAttackState(stateMachine);

        stateMachine.AddAnyTransition(_locoState, new FuncPredicate(() => !isFoundPlayer));
        stateMachine.AddAnyTransition(_chaseState, new FuncPredicate(() => isFoundPlayer && !isInRotateRad && !isAttack));
        stateMachine.AddAnyTransition(_rotateState, new FuncPredicate(() => isInRotateRad && !isAttack));
        stateMachine.AddAnyTransition(_attackState, new FuncPredicate(() => isAttack && !hurtTimer.isTickin));
        stateMachine.AddTransition(_attackState, _rotateState, new FuncPredicate(() => !isAttack));

        stateMachine.SetState(_locoState);
    }

    protected override void SethpSliderPos()
    {

    }
}

public enum EPriestAni
{
    WalkBlend,
    RotateBlend,
    Death,
    Groggy,
    InPlaceJumpAttack,
    ForwardJumpAttack,
    SoccerKick,
    //
    AttackRun,
    HeavyFootStep,
    Combo1,
    Combo2
}

public class PriestAniDB
{
    public Dictionary<EPriestAni, int> aniHashs { get; private set; }
    public Dictionary<EPriestAni, int> aniParam { get; private set; }
    public int[] attackClips { get; private set; }

    public PriestAniDB()
    {
        aniHashs = new Dictionary<EPriestAni, int>();
        aniParam = new Dictionary<EPriestAni, int>();

        //애니메이션 클립
        aniHashs.Add(EPriestAni.WalkBlend, Animator.StringToHash("Base Layer.Walk Blend"));
        aniHashs.Add(EPriestAni.RotateBlend, Animator.StringToHash("Base Layer.Rotate Blend"));
        aniHashs.Add(EPriestAni.Groggy, Animator.StringToHash("Base Layer.StartGroggy"));
        aniHashs.Add(EPriestAni.Death, Animator.StringToHash("Base Layer.Death"));
        aniHashs.Add(EPriestAni.InPlaceJumpAttack, Animator.StringToHash("Base Layer.InPlaceJumpAttack"));
        aniHashs.Add(EPriestAni.ForwardJumpAttack, Animator.StringToHash("Base Layer.ForwardJumpAttack"));
        aniHashs.Add(EPriestAni.SoccerKick, Animator.StringToHash("Base Layer.SoccerKick"));
        aniHashs.Add(EPriestAni.AttackRun, Animator.StringToHash("Base Layer.AttackRun"));
        aniHashs.Add(EPriestAni.HeavyFootStep, Animator.StringToHash("Base Layer.HeavyFootStep"));
        aniHashs.Add(EPriestAni.Combo1, Animator.StringToHash("Base Layer.Combo1"));
        aniHashs.Add(EPriestAni.Combo2, Animator.StringToHash("Base Layer.Combo2"));


        //애니 param
        aniParam.Add(EPriestAni.WalkBlend, Animator.StringToHash("WalkBlend"));
        aniParam.Add(EPriestAni.RotateBlend, Animator.StringToHash("RotateBlend"));
    }
}