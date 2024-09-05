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
    LOCOSPEED,
    ATTACK,
    JUMP,
    RUNJUMP,
    FALL,
    LAND,
    ISGROUND
};

public enum EPlayerAniTriggerOrBool
{
    ISGROUND,
    ISFALL
};


/// <summary>
/// PlayerAnimationData class
/// �ִϸ��̼� �����͸� Dictionary ���·� ����, �����ʹ� ����� �Է�
/// </summary>
public class PlayerAnimationData
{
    //Animation Hash
    public Dictionary<EPlayerState, int> _DTaniClipID { get; private set; }
    public Dictionary<EPlayerAniTriggerOrBool, int> _DTaniTriggerOrBoolID { get; private set; }

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
        _DTaniTriggerOrBoolID = new Dictionary<EPlayerAniTriggerOrBool, int>();

        //Clip ID
        _DTaniClipID.Add(EPlayerState.LOCO, Animator.StringToHash("Loco"));
        _DTaniClipID.Add(EPlayerState.LOCOSPEED, Animator.StringToHash("Speed"));
        _DTaniClipID.Add(EPlayerState.ATTACK, Animator.StringToHash("Attack"));
        _DTaniClipID.Add(EPlayerState.JUMP, Animator.StringToHash("InAir.Jump"));
        _DTaniClipID.Add(EPlayerState.RUNJUMP, Animator.StringToHash("InAir.RunningJump"));
        _DTaniClipID.Add(EPlayerState.FALL, Animator.StringToHash("Falling"));
        _DTaniClipID.Add(EPlayerState.LAND, Animator.StringToHash("InAir.Falling To Landing"));

        //Trigger Name
        _DTaniTriggerOrBoolID.Add(EPlayerAniTriggerOrBool.ISGROUND, Animator.StringToHash("IsGround"));
        _DTaniTriggerOrBoolID.Add(EPlayerAniTriggerOrBool.ISFALL, Animator.StringToHash("IsFall"));
    }
}