using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerinputSystem : MonoBehaviour
{
    [Header("�÷��̾� input values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;

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

    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() > 0f)
        sprint = true;

        else
            sprint = false;
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
