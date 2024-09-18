using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// ���� �÷��̾ ���� �ִ� �κ��丮
/// </summary>
[Serializable]
public class InventoryManager : MonoBehaviour, IInitManager
{
    [SerializeField]
    private int gearInvSize = 20;// �κ��丮 ����Ʈ�� ũ�⸦ ����
    [SerializeField]
    private int consumeInvSize = 20;// �κ��丮 ����Ʈ�� ũ�⸦ ����
    [SerializeField]
    private int missionInvSize = 20;// �κ��丮 ����Ʈ�� ũ�⸦ ����


    [SerializeField]
    private InventorySystem invSys_Gear; // ��� �κ�
    [SerializeField]
    private InventorySystem invSys_Consume; // �Һ� �κ�
    [SerializeField]
    private InventorySystem invSys_mission; // �̼� �κ�

    public InventorySystem InvSys_Gear => invSys_Gear;
    public InventorySystem InvSys_Consume => invSys_Consume;
    public InventorySystem InvSys_mission => invSys_mission;

    public int wallet;

    public UnityAction<InventorySystem> OnDynamicInventoryDisplayRequest;// �κ��丮�� UI�� ��ȣ�ۿ��� ���� ��

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
