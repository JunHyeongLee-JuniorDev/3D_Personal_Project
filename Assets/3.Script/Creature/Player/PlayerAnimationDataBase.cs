using System;
using System.Collections.Generic;
using UnityEngine;

// Enum을 key로 그리고 
public enum EPlayerState
{
    LOCO,
    LOCOSPEED,
    ATTACK,
    MELEEATTACK1,
    MELEEATTACK2,
    JUMP,
    RUNJUMP,
    FALL,
    BATTLE
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

[Serializable]
public class PlayerAnimationDataBase
{
    //Animation Hash
    public Dictionary<EPlayerState, int> _DTaniClipID { get; private set; }
    public Dictionary<EPlayerAniParam, int> _DTaniBoolID { get; private set; }

    public int[] _comboClip = new int[2];

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

        _DTaniClipID = new Dictionary<EPlayerState, int>();
        _DTaniBoolID = new Dictionary<EPlayerAniParam, int>();

        //Clip ID
        _DTaniClipID.Add(EPlayerState.LOCO, Animator.StringToHash("Base Layer.Loco"));
        _DTaniClipID.Add(EPlayerState.LOCOSPEED, Animator.StringToHash("Speed"));
        _DTaniClipID.Add(EPlayerState.MELEEATTACK1, Animator.StringToHash("Base Layer.Attack.Sword And Shield Slash1"));
        _DTaniClipID.Add(EPlayerState.MELEEATTACK2, Animator.StringToHash("Base Layer.Attack.Sword And Shield Slash2"));
        _DTaniClipID.Add(EPlayerState.JUMP, Animator.StringToHash("Base Layer.InAir.Jump"));
        _DTaniClipID.Add(EPlayerState.RUNJUMP, Animator.StringToHash("Base Layer.InAir.RunningJump"));
        _DTaniClipID.Add(EPlayerState.BATTLE, Animator.StringToHash("Base Layer.Battle"));

        //Ani Parameter
        _DTaniBoolID.Add(EPlayerAniParam.ISGROUND, Animator.StringToHash("IsGround"));
        _DTaniBoolID.Add(EPlayerAniParam.ISFALL, Animator.StringToHash("IsFall"));
        _DTaniBoolID.Add(EPlayerAniParam.BATTLEX, Animator.StringToHash("BattleX"));
        _DTaniBoolID.Add(EPlayerAniParam.BATTLEY, Animator.StringToHash("BattleY"));

        //combo clip
        _comboClip[0] = _DTaniClipID[EPlayerState.MELEEATTACK1];
        _comboClip[1] = _DTaniClipID[EPlayerState.MELEEATTACK2];
    }
}