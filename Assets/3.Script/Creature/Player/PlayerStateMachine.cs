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
    ATTACK
};

/// <summary>
/// PlayerAnimationData class
/// 애니메이션 데이터를 Dictionary 형태로 보관, 데이터는 수기로 입력
/// </summary>
public class PlayerAnimationData
{
    //Animation Hash
    public Dictionary<EPlayerState, int> _DTaniClipID { get; private set; }
    public int attackTriggerID = Animator.StringToHash("Attack");

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

        _DTaniClipID.Add(EPlayerState.LOCO, Animator.StringToHash("Speed"));
        _DTaniClipID.Add(EPlayerState.ATTACK, Animator.StringToHash("Attack"));
    }
}