using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerStateMachine : StateMachine
{
    public PlayerController player {  get; private set; }
    public PlayerStateMachine(PlayerController player)
    {
        this.player = player;
    }
}