using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerBaseState : IState
{
    public PlayerStateMachine stateMachine; // µð¹ö±ë¿ë
    protected readonly PlayerGroundData groundData;
    protected readonly PlayerAirData airData;
    protected PlayerController player;
    protected Animator animator;
    protected Dictionary<EPlayerState, int> DTAniClipID;
    protected InputActionAsset inputActions;
    protected InputAction inputAction;
    protected float aniFadeDuration = 0.1f;


    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        player = stateMachine.player;
        groundData = player.m_PhysicsData.GroundedData;
        airData = player.m_PhysicsData.AirData;
        animator = player.m_animator;
        DTAniClipID = player.m_aniData._DTaniClipID;
        inputActions = player.m_playerInput.actions;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {
        
    }


    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
        
    }

    public virtual void OnHurt()
    {
        throw new System.NotImplementedException();
    }
}
