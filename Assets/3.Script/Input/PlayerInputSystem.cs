using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerinputSystem : MonoBehaviour
{
    [Header("플레이어 input values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint = false;
    public bool fire;
    public PlayerController player;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>().normalized;

        if (context.started)
        {
            ChangePlayerMoveState();
        }

        else if (context.canceled)
        {
            ChangePlayerMoveState();
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            fire = true;
            ChangePlayerMoveState();
        }
    }


    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            sprint = true;
            ChangePlayerMoveState();
        }

        else if (context.canceled)
        {
            sprint = false;
            ChangePlayerMoveState();
        }

    }

    private void OnApplicationFocus(bool focus)
    {
        CursorLock();
    }

    private void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ChangePlayerMoveState()
    {
        TurnOffAniBools();

        if (!fire)
        {
            if (move.Equals(Vector2.zero))
            {
                player.m_StateMachine.ChangeState(player.m_StateMachine.idleState);
            }

            else
            {
                if (sprint)
                {
                    player.m_StateMachine.ChangeState(player.m_StateMachine.runState);
                }

                else
                {
                    player.m_StateMachine.ChangeState(player.m_StateMachine.walkState);
                }
            }
        }

        else
        {
            player.m_StateMachine.ChangeState(player.m_StateMachine.attackState);
        }

    }

    private void TurnOffAniBools()
    {
        AnimatorStateInfo animatorStateInfo = player.m_animator.GetCurrentAnimatorStateInfo(0);

        if (fire && animatorStateInfo.normalizedTime >= 1f)
        {
            fire = false;
            player.m_StateMachine.ChangeState(player.m_StateMachine.idleState);
        }
    }
}
