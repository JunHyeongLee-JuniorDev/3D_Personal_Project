using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventorySystem
{
    [SerializeField]
    private List<InventorySlot> inventorySlots;


    public List<InventorySlot> InventorySlots => inventorySlots;

    public int InventorySize => InventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    /// <summary>
    /// 만약 인벤토리의 사이즈를 벗어나면 false를 반환하며 담기지 않음
    /// </summary>
    /// <returns></returns>
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        if (IsContainsItem(itemToAdd, out List<InventorySlot> invSlot))
        {// 같은 아이템이 인벤토리에 있는지 확인 true
            //invSlot.AddToStack(amountToAdd);
            //OnInventorySlotChanged?.Invoke(invSlot);
            return true;
        }

        else if (HasFreeSlot(out InventorySlot freeSlot))
        {// 남은 슬롯이 있는지 true
            freeSlot = new InventorySlot(itemToAdd, amountToAdd);
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }

        return false; // 넣을 수 없다면 false
    }

    public bool IsContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot)
    {
        invSlot = inventorySlots;
        return false;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = null;
        return false;
    }

}
