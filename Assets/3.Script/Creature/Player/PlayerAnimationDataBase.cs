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
    ROLL,
    GRAB
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
    public Dictionary<EPlayerAni, int> _DTaniClipID { get; private set; }
    public Dictionary<EPlayerAniParam, int> _DTaniBoolID { get; private set; }

    public string[] meleeAtackClips;

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
        _DTaniBoolID = new Dictionary<EPlayerAniParam, int>();
        meleeAtackClips = new string[2];

        //Clip ID
        _DTaniClipID.Add(EPlayerAni.LOCO, Animator.StringToHash("Base Layer.LocoState.Loco"));
        _DTaniClipID.Add(EPlayerAni.GRAB, Animator.StringToHash("Base Layer.LocoState.GetItem"));
        _DTaniClipID.Add(EPlayerAni.LOCOSPEED, Animator.StringToHash("Speed"));
        _DTaniClipID.Add(EPlayerAni.JUMP, Animator.StringToHash("Base Layer.InAir.Jump"));
        _DTaniClipID.Add(EPlayerAni.RUNJUMP, Animator.StringToHash("Base Layer.InAir.RunningJump"));
        _DTaniClipID.Add(EPlayerAni.BATTLE, Animator.StringToHash("Base Layer.Battle.Battle"));
        _DTaniClipID.Add(EPlayerAni.BLOCK, Animator.StringToHash("LeftHand.Sword And Shield Block"));
        _DTaniClipID.Add(EPlayerAni.ROLL, Animator.StringToHash("Base Layer.Battle.Stand To Roll"));

        //Ani Parameter
        _DTaniBoolID.Add(EPlayerAniParam.ISGROUND, Animator.StringToHash("IsGround"));
        _DTaniBoolID.Add(EPlayerAniParam.ISFALL, Animator.StringToHash("IsFall"));
        _DTaniBoolID.Add(EPlayerAniParam.BATTLEX, Animator.StringToHash("BattleX"));
        _DTaniBoolID.Add(EPlayerAniParam.BATTLEY, Animator.StringToHash("BattleY"));

    }
}