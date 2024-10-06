using System;
using System.Collections.Generic;
using UnityEngine;

// Enum을 key로 그리고 
public enum EPlayerAni
{
    LOCO,
    LOCOSPEED,
    JUMP,
    RUNJUMP,
    FALL,
    BATTLE,
    BLOCK,
    BLOCKIMAPCT,
    ROLL,
    GRAB,
    SwordSkill,
    AxeSkill,
    MagicSkill,
    Hit,
    Death
};

/// <summary>
/// 트리거 string Hash도 포함되어 있습니다.
/// </summary>
public enum EPlayerAniParam
{
    ISGROUND,
    ISFALL,
    BATTLEX,
    BATTLEY
};

/// <summary>
/// PlayerAnimationDataBase class
/// 애니메이션 데이터를 Dictionary 형태로 보관, 데이터는 수기로 입력
/// </summary>

public class PlayerAnimationDataBase
{
    //Animation Hash
    public Dictionary<EPlayerAni, int> _DTaniClipID { get; private set; }
    public Dictionary<EPlayerAniParam, int> _DTaniParamID { get; private set; }

    public int[] meleeAttackClips {  get; private set; }
    public int[] swordAttackClips { get; private set; }
    public int[] axeAttackClips { get; private set; }
    public int[] magicAttackClips { get; private set; }

    public readonly float meleeComboCool = 0.5f;
    public readonly float swordComboCool = 0.5f;
    public readonly float axeComboCool   = 0.5f;
    public readonly float magicComboCool = 0.8f;

    public readonly float meleeFadeDuration = 0.2f;
    public readonly float swordFadeDuration = 0.25f;
    public readonly float axeFadeDuration = 0.25f;
    public readonly float magicFadeDuration = 0.2f;

    /// <summary>
    /// Initialize 애니메이션 데이터를 Hash값으로 바꿈
    /// </summary>
    public void Initialize()
    {
        /*
         string 값을 주고 받는 과정이 느리기에 hash값을 캐싱한다.
        string 을 한글자씩 비교하기 때문에
        그래서 hash로 바로 찾을 수 있도록
         */

        _DTaniClipID = new Dictionary<EPlayerAni, int>();
        _DTaniParamID = new Dictionary<EPlayerAniParam, int>();
        meleeAttackClips = new int[2];
        swordAttackClips = new int[2];
        axeAttackClips = new int[2];
        magicAttackClips = new int[2];

        //Clip ID
        _DTaniClipID.Add(EPlayerAni.LOCO, Animator.StringToHash("Base Layer.LocoState.Loco"));
        _DTaniClipID.Add(EPlayerAni.GRAB, Animator.StringToHash("RightHand.GetItem"));
        _DTaniClipID.Add(EPlayerAni.LOCOSPEED, Animator.StringToHash("Speed"));
        _DTaniClipID.Add(EPlayerAni.JUMP, Animator.StringToHash("Base Layer.InAir.Jump"));
        _DTaniClipID.Add(EPlayerAni.RUNJUMP, Animator.StringToHash("Base Layer.InAir.RunningJump"));
        _DTaniClipID.Add(EPlayerAni.BATTLE, Animator.StringToHash("Base Layer.Battle.Battle"));
        _DTaniClipID.Add(EPlayerAni.BLOCK, Animator.StringToHash("LeftHand.Sword And Shield Block"));
        _DTaniClipID.Add(EPlayerAni.BLOCKIMAPCT, Animator.StringToHash("LeftHand.Shield Impact"));
        _DTaniClipID.Add(EPlayerAni.ROLL, Animator.StringToHash("Base Layer.Battle.Stand To Roll"));
        _DTaniClipID.Add(EPlayerAni.Hit, Animator.StringToHash("Base Layer.Hit"));
        _DTaniClipID.Add(EPlayerAni.Death, Animator.StringToHash("Base Layer.Death"));

        //스킬들 필요!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        _DTaniClipID.Add(EPlayerAni.SwordSkill, Animator.StringToHash("Sword Skill"));
        _DTaniClipID.Add(EPlayerAni.AxeSkill, Animator.StringToHash("Axe Skill"));
        _DTaniClipID.Add(EPlayerAni.MagicSkill, Animator.StringToHash("Magic Skill"));

        //Ani Parameter
        _DTaniParamID.Add(EPlayerAniParam.ISGROUND, Animator.StringToHash("IsGround"));
        _DTaniParamID.Add(EPlayerAniParam.ISFALL, Animator.StringToHash("IsFall"));
        _DTaniParamID.Add(EPlayerAniParam.BATTLEX, Animator.StringToHash("BattleX"));
        _DTaniParamID.Add(EPlayerAniParam.BATTLEY, Animator.StringToHash("BattleY"));

        //Attack Clips
        meleeAttackClips[0] = Animator.StringToHash("Punch");
        meleeAttackClips[1] = Animator.StringToHash("Punch1");
        swordAttackClips[0] = Animator.StringToHash("Sword And Shield Slash1");
        swordAttackClips[1] = Animator.StringToHash("Sword And Shield Slash2");
        axeAttackClips[0] = Animator.StringToHash("Axe Normal Attack1");
        axeAttackClips[1] = Animator.StringToHash("Axe Normal Attack2");
        magicAttackClips[0] = Animator.StringToHash("Magic Normal Attack");
        magicAttackClips[1] = Animator.StringToHash("Magic Normal Attack1");
    }
}