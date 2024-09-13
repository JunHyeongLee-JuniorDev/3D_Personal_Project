using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DynamicInventoryDisplay : MonoBehaviour
{
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;
    protected Transform viewerTransform;
    [SerializeField] protected InventorySlot_UI slotPrefab;

    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    [field : SerializeField]
    public EItemType invType_UI { get; private set; }

    protected virtual void Start()
    {
        switch (invType_UI)
        {
            case EItemType.GEAR:
                inventorySystem = InventoryManager.instance.InvSys_Gear;
                break;
            case EItemType.MISSION:
                inventorySystem = InventoryManager.instance.InvSys_mission;
                break;
            case EItemType.CONSUME:
                inventorySystem = InventoryManager.instance.InvSys_Consume;
                break;
            default:
                Debug.LogWarning("�Ҵ���� ���� �κ��丮 type");
                break;
        }
        inventorySystem.OnInventorySlotChanged += UpdateSlot;
        AssignSlot(inventorySystem);
    }

    public void AssignSlot(InventorySystem invToDisplay)
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

    public void SlotRightClicked(InventorySlot_UI clickedUISlot)
    {
        if (clickedUISlot.AssignedInventorySlot.ItemData == null) return;

        //item ������ �����
    }

    public void SlotLeftClicked(InventorySlot_UI clickedUISlot)
    {
        if (clickedUISlot.AssignedInventorySlot.ItemData == null) return;

        //������ ��ȣ�ۿ� UI�� �����
    }

}
