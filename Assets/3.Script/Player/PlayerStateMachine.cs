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

    public Vector2 MovementInput;
    
    public PlayerStateMachine(PlayerController player)
    {
        this.player = player;

        idleState = new PlayerIdleState(this);
        walkState = new PlayerWalkState(this);
        runState = new PlayerRunState(this);
    }
}

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        groundData = stateMachine.player.m_PhysicsData.GroundedData;
    }

    public virtual void Enter()
    {
        AddInputActionCallBack();
    }

    public virtual void Exit()
    {
        RemoveInputActionCallBack();
    }

    public virtual void HandleInput()
    {
        ReadMoveInput();
    }

    public virtual void PhysicsUpdate()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        stateMachine.player.Move();
    }

    protected virtual void AddInputActionCallBack()
    {

    }
    protected virtual void RemoveInputActionCallBack()
    {

    }

    private void ReadMoveInput()
    {
        stateMachine.MovementInput =
            stateMachine.player.m_input.move;
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.player.m_animator.Play(animationHash);
    }
}

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) 
    { 

    }

    public override void Enter()
    {
        stateMachine.player.m_targetSpeed = 0f;
        base.Enter();
        StartAnimation(stateMachine.player.m_aniData.m_animIDIdle);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.player.m_targetSpeed = stateMachine.player.m_PhysicsData.GroundedData.
                                            BaseSpeed;
        base.Enter();
        StartAnimation(stateMachine.player.m_aniData.m_animIDWalk);
    }
}

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.player.m_targetSpeed
             = stateMachine.player.m_PhysicsData.GroundedData.sprintSpeed;

        base.Enter();
        StartAnimation(stateMachine.player.m_aniData.m_animIDSprint);
    }
}

public abstract class StateMachine
{
    protected IState currentState;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();

        currentState = newState;

        currentState?.Enter();
    }
    public void Update()
    {
        currentState?.Update();
    }
    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}


public interface IState
{
    public void Enter();
    public void Exit();
    public void Update();
    public void PhysicsUpdate();
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
         string 값을 주고 받는 과정이 느리기에 hash값을 캐싱한다.
        string 을 한글자씩 비교하기 때문에
        그래서 hash로 바로 찾을 수 있도록
         */
        m_animIDWalk = Animator.StringToHash(m_aniNameWalk);
        m_animIDSprint = Animator.StringToHash(m_aniNameSprint);
        m_animIDIdle = Animator.StringToHash(m_aniNameIdle);
        m_animIDAttack = Animator.StringToHash(m_aniNameAttack);
    }
}