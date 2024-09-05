using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerStateMachine : StateMachine
{
    public PlayerController player;

    
    public PlayerStateMachine(PlayerController player)
    {
        this.player = player;
    }
}
// Enum을 key로 그리고 
public enum EPlayerState
{
    LOCO,
    LOCOSPEED,
    ATTACK,
    JUMP,
    RUNJUMP,
    FALL,
    LAND,
    ISGROUND
};

public enum EPlayerAniTriggerOrBool
{
    ISGROUND,
    ISFALL
};


/// <summary>
/// PlayerAnimationData class
/// 애니메이션 데이터를 Dictionary 형태로 보관, 데이터는 수기로 입력
/// </summary>
public class PlayerAnimationData
{
    //Animation Hash
    public Dictionary<EPlayerState, int> _DTaniClipID { get; private set; }
    public Dictionary<EPlayerAniTriggerOrBool, int> _DTaniTriggerOrBoolID { get; private set; }

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

        _DTaniClipID =new Dictionary<EPlayerState, int>();
        _DTaniTriggerOrBoolID = new Dictionary<EPlayerAniTriggerOrBool, int>();

        //Clip ID
        _DTaniClipID.Add(EPlayerState.LOCO, Animator.StringToHash("Loco"));
        _DTaniClipID.Add(EPlayerState.LOCOSPEED, Animator.StringToHash("Speed"));
        _DTaniClipID.Add(EPlayerState.ATTACK, Animator.StringToHash("Attack"));
        _DTaniClipID.Add(EPlayerState.JUMP, Animator.StringToHash("InAir.Jump"));
        _DTaniClipID.Add(EPlayerState.RUNJUMP, Animator.StringToHash("InAir.RunningJump"));
        _DTaniClipID.Add(EPlayerState.FALL, Animator.StringToHash("Falling"));
        _DTaniClipID.Add(EPlayerState.LAND, Animator.StringToHash("InAir.Falling To Landing"));

        //Trigger Name
        _DTaniTriggerOrBoolID.Add(EPlayerAniTriggerOrBool.ISGROUND, Animator.StringToHash("IsGround"));
        _DTaniTriggerOrBoolID.Add(EPlayerAniTriggerOrBool.ISFALL, Animator.StringToHash("IsFall"));
    }
}