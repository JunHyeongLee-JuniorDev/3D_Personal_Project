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
public class PlayerAnimationData
{
    //Animation String
    private string m_aniNameWalk = "Walk";
    private string m_aniNameSprint = "Sprint";
    private string m_aniNameIdle = "Idle";
    private string m_aniNameAttack = "Attack";

    //Animation Hash
    public int m_animIDWalk { get; private set; }
    public int m_animIDSprint { get; private set; }
    public int m_animIDIdle { get; private set; }
    public int m_animIDAttack { get; private set; }

    public void Initialize()
    {
        /*
         string ���� �ְ� �޴� ������ �����⿡ hash���� ĳ���Ѵ�.
        string �� �ѱ��ھ� ���ϱ� ������
        �׷��� hash�� �ٷ� ã�� �� �ֵ���
         */
        m_animIDWalk = Animator.StringToHash(m_aniNameWalk);
        m_animIDSprint = Animator.StringToHash(m_aniNameSprint);
        m_animIDIdle = Animator.StringToHash(m_aniNameIdle);
        m_animIDAttack = Animator.StringToHash(m_aniNameAttack);
    }
}