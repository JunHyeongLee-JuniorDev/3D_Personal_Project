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

    public event Action<Vector2> moveEvent;

    public void OnMove(InputAction.CallbackContext context)

    {
        move = context.ReadValue<Vector2>();
        moveEvent.Invoke(move);
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 _lookInput = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {

    }


}
