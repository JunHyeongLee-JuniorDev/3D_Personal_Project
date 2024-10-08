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
    private Coroutine staminaCash;

    private void Start()
    { 
        GetComponent<PlayerInput>().actions["T"].started -= OnPressT;// for debug
        GetComponent<PlayerInput>().actions["T"].started += OnPressT;// for debug
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

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (player.m_PlayerData.currentStamina <= 0.0f) return;

        if (context.started)
        {
            player.isSprint = true;
            if(staminaCash != null) StopCoroutine(staminaCash);
            staminaCash = StartCoroutine(ReduceStamina_co());
        }

        else if (context.canceled)
        {
            if (staminaCash != null) StopCoroutine(staminaCash);
            player.isSprint = false;
        }
    }

    private IEnumerator ReduceStamina_co()
    {
        while (true)
        {
            player.m_PlayerData.currentStamina -= player.staminaCost * Time.deltaTime;
            if (player.m_PlayerData.currentStamina <= 0.0f)
            {
                player.m_PlayerData.currentStamina = 0.0f;
                player.isSprint = false;
                yield break;
            }
            player.staminaFillTimer.StartTimer(() => { });
            Managers.Instance.Game.OnStaminaChange?.Invoke();
            yield return null;
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
            Managers.Instance.Inventory.PlayerData.ChangePotion();
            Managers.Instance.Inventory.PlayerData.OnChangeHealPotion?.Invoke();
        }
    }

    public void OnPressT(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            player.isDrinkPotion = true;
            Managers.Instance.Inventory.PlayerData.DrinkPotion();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
