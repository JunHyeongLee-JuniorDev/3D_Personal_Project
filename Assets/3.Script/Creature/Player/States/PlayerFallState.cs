using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) {}

    private float currentHorizontalSpeed;
    private Vector2 lastMoveDirection;
    private InputAction fallAction;

    public override void Enter()
    {
        base.Enter();
        lastMoveDirection = player.m_input.move;
        inputActions["Move"].Disable();
        inputActions["Fire"].Disable();
    }

    public override void Update()
    {
        base.Update();

        if (player.m_targetSpeed > 5f)
        {
            player.m_targetSpeed -= 15f * Time.deltaTime;
        }

        else if (player.m_targetSpeed > 0.0f)
            player.m_targetSpeed -= 2.3f * Time.deltaTime;


        Move();
        JumpAndGravity();
    }

    public override void Exit()
    {
        base.Exit();
        inputActions["Move"].Enable();
        inputActions["Fire"].Enable();
    }
}
