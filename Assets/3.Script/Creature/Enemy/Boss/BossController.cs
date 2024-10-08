using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(NavMeshAgent))]
public class BossController : MonsterController
{
    /*
     * 기존 State를 그대로 쓰되 patrol state는 빼기로 하자
     * 같은 surface에 있으면 target를 바로 인식하도록...
     */

    //Data
    public PriestAniDB aniDB { get; private set; }
    public float currentGroggy;
    public readonly float maxGroggy = 100.0f;
    private AudioSource audioSource;

    [field: SerializeField] public float walkSpeed { get; private set; } = 5.0f;
    [field: SerializeField] public float chargeSpeed { get; private set; } = 30.0f;
    [field: SerializeField] public float rotateSpeed { get; private set; } = 30.0f;
    public bool isChargeState;
    public bool isPlayerEnter = false;

    [field : SerializeField]public LayerMask wallLayer { get; private set; }


    [SerializeField] private Vector3 wallCheckOffset;

    //EFF
    [field : SerializeField] public ParticleSystem wallCrashEFF { get; private set; }


    //Attack collider
    [field: SerializeField] public BoxCollider[] attackCols { get; private set; }
    private int attackColIndex;
    private ParticleSystem jumpDust;
    /// <summary>
    /// 보스 공격 메소드에서 갱신
    /// </summary>
    /// <param name="newIndex">새로운 공격 인덱스</param>
    public void AssignAttackColIndex(int newIndex) { attackColIndex = newIndex; }

    protected override void Start()
    {
        navAI = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        weaponTrigger = GetComponentInChildren<MonsterWeaponTrigger>();
        jumpDust = attackCols[1].GetComponentInChildren<ParticleSystem>();
        bloodEFF = transform.GetChild(5).GetComponent<ParticleSystem>();
        hitBox = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();

        //Inits
        aniDB = new PriestAniDB();
        attackTimer = new Timer(0.0f, this);
        hurtTimer = new Timer(5.0f, this);
        deathTimer = new Timer(5.0f, this);
        isPlayerEnter = false;

        player = Managers.Instance.Game.playerController.transform; //메니저에서 할당하는 것으로 바꾸자

        hpSlider = Instantiate(hpPrefabs);
        hpSlider.gameObject.SetActive(false);
        InitStatData();
        statData = Managers.Instance.Data.LoadMonsterData(statData.monsterID, statData);
        if (statData.isDead) gameObject.SetActive(false);

        Managers.Instance.Game.playerController.OnPlayerDead -= OnPlayerDead;
        Managers.Instance.Game.playerController.OnPlayerDead += OnPlayerDead;
        TurnOffAllBossAttackCol();


        currentGroggy = 0.0f;
        navAI.updateRotation = false;

        //Init States
        stateMachine = new MonsterStateMachine(this);
        var _restState = new BossRestState(stateMachine);
        var _locoState = new BossLocoState(stateMachine);
        var _chargeState = new BossChargeState(stateMachine);
        var _rotateState = new BossRotateState(stateMachine);
        var _attackState = new BossAttackState(stateMachine);
        var _deadState = new MonsterDeathState(stateMachine);

        stateMachine.AddAnyTransition(_restState, new FuncPredicate(() => !isPlayerEnter && !isDead));
        stateMachine.AddAnyTransition(_locoState, new FuncPredicate(() => !isInRotateRad && !isChargeState && isPlayerEnter && !isDead));
        stateMachine.AddAnyTransition(_chargeState, new FuncPredicate(() => isChargeState && !isDead));
        stateMachine.AddAnyTransition(_rotateState, new FuncPredicate(() => isInRotateRad && !isAttack && !isChargeState && !isDead));
        stateMachine.AddAnyTransition(_attackState, new FuncPredicate(() => isAttack && !hurtTimer.isTickin && !isDead));
        stateMachine.AddAnyTransition(_deadState, new FuncPredicate(() => isDead));

        stateMachine.SetState(_restState);
    }

    protected override void Update()
    {
        stateMachine.Update();
        ReduceGroggy();
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
        statData.currentHealth = 300.0f;
        statData.maxHealth = 300.0f;
        statData.isDead = false;
        statData.isBoss = true;
    }

    public void CancelAllConditions()
    {
        isPlayerEnter = false;
        isInRotateRad = false;
        isChargeState = false;
        isAttack = false;
    }

    public void TurnOnBossAttackCol()
    {
        switch(attackColIndex)
        {
            case 1:
                jumpDust.Play();
                break;
        }

        attackCols[attackColIndex].enabled = true;
    }
    public void TurnOffBossAttackCol()
    {
        attackCols[attackColIndex].enabled = false;
    }

    public void TurnOffAllBossAttackCol()
    {
        foreach (var col in attackCols)
        {
            col.enabled = false;
        }
    }

    public void UpdateGroggy(float addValue)
    {
        if (hurtTimer.isTickin) return;
        if(currentGroggy +  addValue >= maxGroggy) currentGroggy = maxGroggy;
        currentGroggy += addValue;
    }

    private void ReduceGroggy()
    {
        if (currentGroggy <= 0.0f) currentGroggy = 0.0f;
        else currentGroggy -= Time.deltaTime;
    }

    public void PlayBossFootStep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight >= 0.5f)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(Managers.Instance.Sound.GetOrAddAudioClip("Monster/BossStep"));
        }
    }

    public void PlayCrashSound()
    {
        audioSource.PlayOneShot(Managers.Instance.Sound.GetOrAddAudioClip("Monster/BossRunCrash"));
    }

    private void OnDisable()
    {
        if (isDead)
        {
            Managers.Instance.Sound.Play("Bgm/Game");
            Destroy(hpSlider.gameObject);
        }
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
        aniHashs.Add(EPriestAni.Block, Animator.StringToHash("Left Hand.Block"));

        //애니 param
        aniParam.Add(EPriestAni.WalkBlend, Animator.StringToHash("WalkBlend"));
        aniParam.Add(EPriestAni.RotateBlend, Animator.StringToHash("RotateBlend"));
        aniParam.Add(EPriestAni.BlendX, Animator.StringToHash("BlendX"));
        aniParam.Add(EPriestAni.BlendY, Animator.StringToHash("BlendY"));
    }
}