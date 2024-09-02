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
    public bool sprint;
    public bool isFired;
    public PlayerController player;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isFired) return;
        move = context.ReadValue<Vector2>().normalized;

        ChangePlayerMoveState();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0f)
        {
            isFired = true;
        }
        player.m_StateMachine.ChangeState(player.m_StateMachine.attackState);
        StartCoroutine(CountAttackAni_co());
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if(isFired) return;

        if (context.ReadValue<float>() > 0f)
            sprint = true;
        
        else
            sprint = false;

        ChangePlayerMoveState();
    }

    private void ChangePlayerMoveState()
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
    private IEnumerator CountAttackAni_co()
    {
        Debug.Log(player.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        yield return new WaitForSeconds(player.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        ChangePlayerMoveState();
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

}
