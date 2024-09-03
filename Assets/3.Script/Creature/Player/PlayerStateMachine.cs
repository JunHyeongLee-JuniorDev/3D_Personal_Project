using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerStateMachine : StateMachine
{
    public PlayerController player;

    public PlayerIdleState idleState;
    public PlayerWalkState walkState;
    public PlayerRunState runState;
    public PlayerAttackState attackState;

    private PlayerinputSystem m_input;

    public Vector2 MovementInput;
    
    public PlayerStateMachine(PlayerController player)
    {
        this.player = player;

        idleState = new PlayerIdleState(this);
        walkState = new PlayerWalkState(this);
        runState = new PlayerRunState(this);
        attackState = new PlayerAttackState(this);
    }
}
// Enum을 key로 그리고 
public enum EPlayerState
{
    IDLE,
    WALK,
    SPRINT,
    ATTACK
};

public class PlayerAnimationData
{
    //Animation Hash
    public Dictionary<EPlayerState, int> _DTaniClip { get; private set; }

    public void Initialize()
    {
        /*
         string 값을 주고 받는 과정이 느리기에 hash값을 캐싱한다.
        string 을 한글자씩 비교하기 때문에
        그래서 hash로 바로 찾을 수 있도록
         */

        _DTaniClip =new Dictionary<EPlayerState, int>();
        
        _DTaniClip.Add(EPlayerState.IDLE, Animator.StringToHash("Idle"));
        _DTaniClip.Add(EPlayerState.WALK, Animator.StringToHash("Walk"));
        _DTaniClip.Add(EPlayerState.SPRINT, Animator.StringToHash("Run"));
        _DTaniClip.Add(EPlayerState.ATTACK, Animator.StringToHash("Attack"));
    }
}