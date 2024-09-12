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
    /// backend에 있는 실제 Slot의 값을 UI Slot에 갱신
    /// </summary>
    /// <param name="updatedSlot"> 새로 받은 아이템 슬롯을 같은 아이템이 있다면 UI에 갱신</param>
    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in slotDictionary)
        {
            if (slot.Value.Equals(updatedSlot)) // 만약 UI에 있는 아이템과 같은 아이템이라면 추가
            {
                slot.Key.UpdateUISlot(updatedSlot);
            }
        }
    }

    public void SlotRightClicked(InventorySlot_UI clickedUISlot)
    {
        if (clickedUISlot.AssignedInventorySlot.ItemData == null) return;

        //item을 어떻게 할 것 인가...
        //
    }

    public void SlotLeftClicked(InventorySlot_UI clickedUISlot)
    {

    }

}
