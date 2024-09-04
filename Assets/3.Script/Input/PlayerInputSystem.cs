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
    public PlayerController player;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>().normalized;
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            player.isBattle = true;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started && !player.isBattle)
        player.isJump = true;
    }


    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            player.isSprint = true;
        }

        else if (context.canceled)
        {
            player.isSprint = false;
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
}
