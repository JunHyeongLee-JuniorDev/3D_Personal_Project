using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// 실제 플레이어가 갖고 있는 인벤토리
/// </summary>
[Serializable]
public class InventoryManager : MonoBehaviour, IInitManager
{
    // 인벤토리 리스트의 크기를 정함------------------------------------------------------------------------

    //플레이어 데이터-------------------------------------------------------------------------------------
    [SerializeField]
    private PlayerData playerData;
    //플레이어 데이터-------------------------------------------------------------------------------------

    // 인벤토리-------------------------------------------------------------------------------------------
    [SerializeField]
    private InventorySystem invSys_Gear; // 기어 인벤
    [SerializeField]
    private InventorySystem invSys_Consume; // 소비 인벤
    [SerializeField]
    private InventorySystem invSys_mission; // 미션 인벤
    // 인벤토리-------------------------------------------------------------------------------------------

    public InventorySystem InvSys_Gear => invSys_Gear;
    public InventorySystem InvSys_Consume => invSys_Consume;
    public InventorySystem InvSys_mission => invSys_mission;
    public PlayerData PlayerData => playerData;

    public int wallet; // 지갑!

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
