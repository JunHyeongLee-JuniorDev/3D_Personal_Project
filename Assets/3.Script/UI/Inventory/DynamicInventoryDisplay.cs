using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DynamicInventoryDisplay : MonoBehaviour
{
    protected PlayerHud_UI playerHud_UI;
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;
    [SerializeField]protected Transform viewerTransform;
    protected InventorySlot_UI slotPrefab;

    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    [field : SerializeField]
    public InventoryType invType_UI { get; private set; }

    public enum InventoryType
    { 
        Equipment,
        Mission,
        Consume
    };

    protected virtual void Start()
    {
        playerHud_UI = GetComponentInParent<PlayerHud_UI>();
        slotPrefab = Resources.Load<InventorySlot_UI>("Prefabs/InventorySlot_UI");

        switch (invType_UI)
        {
            case InventoryType.Equipment:
                inventorySystem = Managers.Instance.Inventory.InvSys_Gear;
                break;
            case InventoryType.Mission:
                inventorySystem = Managers.Instance.Inventory.InvSys_mission;
                break;
            case InventoryType.Consume:
                inventorySystem = Managers.Instance.Inventory.InvSys_Consume;
                break;
            default:
                Debug.LogWarning("�Ҵ���� ���� �κ��丮 type");
                break;
        }
        inventorySystem.OnInventorySlotChanged += UpdateSlot;
        AssignSlot(inventorySystem);
    }

    public virtual void AssignSlot(InventorySystem invToDisplay)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        for (int i = 0; i < invToDisplay.InventorySize; i++)
        {
            var slot_UI = Instantiate(slotPrefab, viewerTransform);
            slotDictionary.Add(slot_UI, invToDisplay.InventorySlots[i]);
            slot_UI.Init(invToDisplay.InventorySlots[i]);
        }
    }

    /// <summary>
    /// backend�� �ִ� ���� Slot�� ���� UI Slot�� ����
    /// </summary>
    /// <param name="updatedSlot"> ���� ���� ������ ������ ���� �������� �ִٸ� UI�� ����</param>
    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in slotDictionary)
        {
            if (slot.Value.Equals(updatedSlot)) // ���� UI�� �ִ� �����۰� ���� �������̶�� �߰�
            {
                slot.Key.UpdateUISlot(updatedSlot);
            }
        }
    }


    public void SlotLeftClicked(InventorySlot_UI clickedUISlot)
    {
        if (clickedUISlot.AssignedInventorySlot.ItemData == null) return;

        switch (clickedUISlot.AssignedInventorySlot.ItemData.ItemType)
        {
            case EItemType.CONSUME:
                Debug.Log("���� ���� �ʿ�");
                break;

            case EItemType.GEAR:
                Debug.Log("���� ���� �ʿ�");
                break;

            case EItemType.MISSION:
                Debug.Log("���� ���� �ʿ�");
                break;
        }
    }
    public void SlotRightClicked(InventorySlot_UI clickedUISlot)
    {
        if (clickedUISlot.AssignedInventorySlot.ItemData == null) return;

        switch (invType_UI)
        {
            case InventoryType.Equipment:
                Debug.Log("��Ŭ�� UI â ����.");
                break;
            case InventoryType.Mission:
                Debug.Log("��Ŭ�� UI â ����.");
                break;
            case InventoryType.Consume:
                Debug.Log("��Ŭ�� UI â ����.");
                break;
            default:
                Debug.LogWarning("�Ҵ���� ���� �κ��丮 type");
                break;
        }
    }

}
