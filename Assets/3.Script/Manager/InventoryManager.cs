using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// ���� �÷��̾ ���� �ִ� �κ��丮
/// </summary>
[Serializable]
public class InventoryManager : MonoBehaviour, IInitManager
{
    // �κ��丮 ����Ʈ�� ũ�⸦ ����------------------------------------------------------------------------

    //�÷��̾� ������-------------------------------------------------------------------------------------
    [SerializeField]
    private PlayerData playerData;
    //�÷��̾� ������-------------------------------------------------------------------------------------

    // �κ��丮-------------------------------------------------------------------------------------------
    [SerializeField]
    private InventorySystem invSys_Gear; // ��� �κ�
    [SerializeField]
    private InventorySystem invSys_Consume; // �Һ� �κ�
    [SerializeField]
    private InventorySystem invSys_mission; // �̼� �κ�
    // �κ��丮-------------------------------------------------------------------------------------------

    public InventorySystem InvSys_Gear => invSys_Gear;
    public InventorySystem InvSys_Consume => invSys_Consume;
    public InventorySystem InvSys_mission => invSys_mission;
    public PlayerData PlayerData => playerData;

    public int wallet; // ����!

    public UnityEvent OnDynamicWeaponChanged;
    public UnityEvent OnDynamicInventoryChanged;

    public void Init()
    {

    }

    public void Clear()
    {

    }

    public void LoadInventory()
    {
        GameSaveData _currentSaveData = Managers.Instance.Data.currentSaveData[Managers.Instance.Data.currentSaveIndex];

        invSys_Consume = _currentSaveData.consumeInvData;
        invSys_Gear = _currentSaveData.equipmentInvData;
        invSys_mission = _currentSaveData.missionInvData;

        playerData = _currentSaveData.savePlayerData;
        wallet = _currentSaveData.savePlayerData.wallet;
    }
}
