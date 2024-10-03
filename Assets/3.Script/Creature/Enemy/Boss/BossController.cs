using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossController : MonsterController
{
    /*
     * ���� State�� �״�� ���� patrol state�� ����� ����
     * ���� surface�� ������ target�� �ٷ� �ν��ϵ���...
     */

    //Data
    public PriestAniDB aniDB { get; private set; }
    public float currentGroggy { get; private set; }
    public readonly float maxGroggy = 100.0f;

    [field: SerializeField] public float walkSpeed { get; private set; } = 5.0f;
    [field: SerializeField] public float chargeSpeed { get; private set; } = 30.0f;
    [field: SerializeField] public float rotateSpeed { get; private set; } = 30.0f;
    public bool isChargeState;
    public bool isPlayerEnter = false;

    [field : SerializeField]public LayerMask wallLayer { get; private set; }


    [SerializeField] private Vector3 wallCheckOffset;


    //EFF
    [field : SerializeField] public ParticleSystem wallCrashEFF { get; private set; }

    protected override void Start()
    {
        navAI = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        weaponTrigger = GetComponentInChildren<MonsterWeaponTrigger>();
        bloodEFF = transform.GetChild(5).GetComponent<ParticleSystem>();
        hitBox = GetComponent<Collider>();

        //Inits
        aniDB = new PriestAniDB();
        attackTimer = new Timer(0.0f, this);
        hurtTimer = new Timer(3.0f, this);
        deathTimer = new Timer(3.0f, this);
        isPlayerEnter = false;
        player = FindObjectOfType<PlayerController>().transform; //�޴������� �Ҵ��ϴ� ������ �ٲ���

        //hpSlider = Instantiate(hpPrefabs;
        InitStatData();
        statData = Managers.Instance.Data.LoadMonsterData(statData.monsterID, statData);
        Managers.Instance.Game.playerController.OnPlayerDead -= OnPlayerDead;
        Managers.Instance.Game.playerController.OnPlayerDead += OnPlayerDead;

        currentGroggy = maxGroggy;
        navAI.updateRotation = false;

        //Init States
        stateMachine = new MonsterStateMachine(this);
        var _restState = new BossRestState(stateMachine);
        var _locoState = new BossLocoState(stateMachine);
        var _chargeState = new BossChargeState(stateMachine);
        var _rotateState = new BossRotateState(stateMachine);
        var _attackState = new BossAttackState(stateMachine);

        stateMachine.AddAnyTransition(_restState, new FuncPredicate(() => !isPlayerEnter));
        stateMachine.AddAnyTransition(_locoState, new FuncPredicate(() => !isInRotateRad && !isChargeState && isPlayerEnter));
        stateMachine.AddAnyTransition(_chargeState, new FuncPredicate(() => isChargeState));
        stateMachine.AddAnyTransition(_rotateState, new FuncPredicate(() => isInRotateRad && !isAttack && !isChargeState));
        stateMachine.AddAnyTransition(_attackState, new FuncPredicate(() => isAttack && !hurtTimer.isTickin));

        stateMachine.SetState(_restState);
    }

    protected override void Update()
    {
        stateMachine.Update();
        transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, navAI.desiredVelocity, _rotLerp));
    }

    public bool IsHitWall()
    {
        return Physics.CheckSphere(transform.position + wallCheckOffset, sphereRad, wallLayer);
    }

    protected override void SethpSliderPos() {}

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + wallCheckOffset, sphereRad);
    }

    protected override void InitStatData()
    {
        statData.currentHealth = 2000.0f;
        statData.maxHealth = 2000.0f;
        statData.isDead = false;
        statData.isBoss = true;
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
    AttackRun,
    HeavyFootStep,
    Combo1,
    Combo2,
    BlendX,
    BlendY,
    Block
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

        //�ִϸ��̼� Ŭ��
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
        aniHashs.Add(EPriestAni.Block, Animator.StringToHash("Left Hand.Block"));

        //�ִ� param
        aniParam.Add(EPriestAni.WalkBlend, Animator.StringToHash("WalkBlend"));
        aniParam.Add(EPriestAni.RotateBlend, Animator.StringToHash("RotateBlend"));
        aniParam.Add(EPriestAni.BlendX, Animator.StringToHash("BlendX"));
        aniParam.Add(EPriestAni.BlendY, Animator.StringToHash("BlendY"));
    }
}