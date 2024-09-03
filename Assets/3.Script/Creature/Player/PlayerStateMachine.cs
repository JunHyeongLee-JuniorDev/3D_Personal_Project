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
// Enum�� key�� �׸��� 
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
         string ���� �ְ� �޴� ������ �����⿡ hash���� ĳ���Ѵ�.
        string �� �ѱ��ھ� ���ϱ� ������
        �׷��� hash�� �ٷ� ã�� �� �ֵ���
         */

        _DTaniClip =new Dictionary<EPlayerState, int>();
        
        _DTaniClip.Add(EPlayerState.IDLE, Animator.StringToHash("Idle"));
        _DTaniClip.Add(EPlayerState.WALK, Animator.StringToHash("Walk"));
        _DTaniClip.Add(EPlayerState.SPRINT, Animator.StringToHash("Run"));
        _DTaniClip.Add(EPlayerState.ATTACK, Animator.StringToHash("Attack"));
    }
}