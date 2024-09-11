using System;
using System.Xml;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int stackSize;

    public InventoryItemData Data => itemData;
    public int StackSize => stackSize;
    public InventorySlot(InventoryItemData source, int amount)
    {
        itemData = source;
        stackSize = amount;
    }

    /// <summary>
    /// 아이템은 없지만 빈 슬롯을 구현해야 할 때
    /// </summary>
    public InventorySlot()
    {
        itemData = null;
        stackSize = -1;
    }

    public void ClearSlot()
    {
        itemData = null;
        stackSize = -1;
    }

    public void UpdateInventorySlot(InventoryItemData Data, int amount)
    {
        itemData = Data;
        stackSize = amount;
    }

    /// <summary>
    /// 다음 메소드를 사용하여 슬롯끼리 더하기가 가능
    /// </summary>
    /// <param name="amountToAdd">더 할 아이템 양</param>
    /// <param name="amountRemaining">남은 slot을 반환</param>
    /// <returns></returns>
    public bool IsroomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = itemData._maxStackSize - stackSize;

        return IsroomLeftInStack(amountToAdd);
    }

    public bool IsroomLeftInStack(int amountToAdd)
    {
        if (stackSize + amountToAdd <= itemData._maxStackSize) return true;

        return false;
    }

    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }

}
