using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    protected float aniFadeDuration = 0.1f;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        groundData = stateMachine.player.m_PhysicsData.GroundedData;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        
    }

    public virtual void HandleInput()
    {
        ReadMoveInput();
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        stateMachine.player.Move();
    }


    private void ReadMoveInput()
    {
        stateMachine.MovementInput =
            stateMachine.player.m_input.move;
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.player.m_animator.CrossFade(animationHash, aniFadeDuration, 0);
    }

    public virtual void OnHurt()
    {
        throw new System.NotImplementedException();
    }
}
