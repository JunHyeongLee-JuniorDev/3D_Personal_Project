using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerinputSystem : MonoBehaviour
{
    [field : Header("플레이어 input values")]
    [field: SerializeField]
    public Vector2 move { get; private set; }
    [field: SerializeField]
    public Vector2 look { get; private set; }
    public PlayerController player;

    private void Start()
    {
        player.m_playerInput.actions["T"].started -= OnPressT;
        player.m_playerInput.actions["T"].started += OnPressT;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started && !player.isAttack)
        {
            player.isAttack = true;
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

    public void OnBattle(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            player.isBattle = !player.isBattle;
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if(context.started)
            player.isRightClicked = true;

        else if (context.canceled)
        player.isRightClicked = false;
    }
    
    public void OnChangeWheel(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("내가 바뀐다. 난 짱멋있다!");
            Managers.Instance.Inventory.PlayerData.ChangePotion();
            Managers.Instance.Inventory.PlayerData.OnChangeHealPotion?.Invoke();
        }
    }

    public void OnPressT(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("T눌림");
            player.isDrinkPotion = true;
            Managers.Instance.Inventory.PlayerData.DrinkPotion();
            Managers.Instance.Inventory.PlayerData.OnRefillStatus?.Invoke(); 
        }
    }
}
