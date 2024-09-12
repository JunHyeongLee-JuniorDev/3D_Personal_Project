using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;

    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {
        
    }

    public abstract void AssignSlot(InventorySystem invToDisplay);

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

        //item�� ��� �� �� �ΰ�...
        //
    }

    public void SlotLeftClicked(InventorySlot_UI clickedUISlot)
    {

    }

}
