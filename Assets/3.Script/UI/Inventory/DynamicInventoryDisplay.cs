using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DynamicInventoryDisplay : MonoBehaviour
{
    private InventorySystem inventorySystem;
    private Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;

    [SerializeField] private InventorySlot_UI slotPrefab;

    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    private void Start()
    {
        
    }

    public void AssignSlot(InventorySystem invToDisplay)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        for (int i = 0; i < invToDisplay.InventorySize; i++)
        {
            var slot_UI = Instantiate(slotPrefab, transform);
            slotDictionary.Add(slot_UI, invToDisplay.InventorySlots[i]);
            slot_UI.Init(invToDisplay.InventorySlots[i]);
        }
    }

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

        //item 설명을 띄워줌
    }

    public void SlotLeftClicked(InventorySlot_UI clickedUISlot)
    {
        if (clickedUISlot.AssignedInventorySlot.ItemData == null) return;

        //아이템 상호작용 UI를 띄워줌
    }

}
