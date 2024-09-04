using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;

public class PlayerStateMachine : StateMachine
{
    public PlayerController player;

    
    public PlayerStateMachine(PlayerController player)
    {
        this.player = player;
    }
}
// Enum�� key�� �׸��� 
public enum EPlayerState
{
    LOCO,
    ATTACK
};

/// <summary>
/// PlayerAnimationData class
/// �ִϸ��̼� �����͸� Dictionary ���·� ����, �����ʹ� ����� �Է�
/// </summary>
public class PlayerAnimationData
{
    //Animation Hash
    public Dictionary<EPlayerState, int> _DTaniClipID { get; private set; }
    public int attackTriggerID = Animator.StringToHash("Attack");

    /// <summary>
    /// Initialize �ִϸ��̼� �����͸� Hash������ �ٲ�
    /// </summary>
    public void Initialize()
    {
        /*
         string ���� �ְ� �޴� ������ �����⿡ hash���� ĳ���Ѵ�.
        string �� �ѱ��ھ� ���ϱ� ������
        �׷��� hash�� �ٷ� ã�� �� �ֵ���
         */

        _DTaniClipID =new Dictionary<EPlayerState, int>();

        _DTaniClipID.Add(EPlayerState.LOCO, Animator.StringToHash("Speed"));
        _DTaniClipID.Add(EPlayerState.ATTACK, Animator.StringToHash("Attack"));
    }
}