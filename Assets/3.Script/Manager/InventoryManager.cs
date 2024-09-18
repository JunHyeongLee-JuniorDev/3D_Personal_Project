using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// 실제 플레이어가 갖고 있는 인벤토리
/// </summary>
[Serializable]
public class InventoryManager : MonoBehaviour, IInitManager
{
    [SerializeField]
    private int gearInvSize = 20;// 인벤토리 리스트의 크기를 정함
    [SerializeField]
    private int consumeInvSize = 20;// 인벤토리 리스트의 크기를 정함
    [SerializeField]
    private int missionInvSize = 20;// 인벤토리 리스트의 크기를 정함


    [SerializeField]
    private InventorySystem invSys_Gear; // 기어 인벤
    [SerializeField]
    private InventorySystem invSys_Consume; // 소비 인벤
    [SerializeField]
    private InventorySystem invSys_mission; // 미션 인벤

    public InventorySystem InvSys_Gear => invSys_Gear;
    public InventorySystem InvSys_Consume => invSys_Consume;
    public InventorySystem InvSys_mission => invSys_mission;

    public int wallet;

    public UnityAction<InventorySystem> OnDynamicInventoryDisplayRequest;// 인벤토리가 UI와 상호작용이 있을 때

    public void Init()
    {
        invSys_Gear = new InventorySystem(gearInvSize);
        invSys_Consume = new InventorySystem(consumeInvSize);
        invSys_mission = new InventorySystem(missionInvSize);
    }

    public void LoadInventory()
    {
        GameSaveData _currentSaveData = Managers.Instance.Data.currentSaveData[Managers.Instance.Data.currentSaveIndex];

        invSys_Gear = new InventorySystem(gearInvSize);
        invSys_Consume = new InventorySystem(consumeInvSize);
        invSys_mission = new InventorySystem(missionInvSize);

        foreach (InventorySlot item in _currentSaveData.equipmentInvData.slots)
        {
            invSys_Gear.AddToInventory(item.ItemData, item.StackSize);
        }

        foreach (InventorySlot item in _currentSaveData.consumeInvData.slots)
        {
            invSys_Consume.AddToInventory(item.ItemData, item.StackSize);
        }

        foreach (InventorySlot item in _currentSaveData.missionInvData.slots)
        {
            invSys_mission.AddToInventory(item.ItemData, item.StackSize);
        }
    }
}
