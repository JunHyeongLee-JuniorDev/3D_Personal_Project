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
    /// ���� �κ��丮�� ����� ����� false�� ��ȯ�ϸ� ����� ����
    /// </summary>
    /// <returns></returns>
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        if (IsContainsItem(itemToAdd, out List<InventorySlot> invSlot))
        {// ���� �������� �κ��丮�� �ִ��� Ȯ�� true
            //invSlot.AddToStack(amountToAdd);
            //OnInventorySlotChanged?.Invoke(invSlot);
            return true;
        }

        else if (HasFreeSlot(out InventorySlot freeSlot))
        {// ���� ������ �ִ��� true
            freeSlot = new InventorySlot(itemToAdd, amountToAdd);
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }

        return false; // ���� �� ���ٸ� false
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
