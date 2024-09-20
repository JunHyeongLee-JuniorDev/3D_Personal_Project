using System;
using System.Xml;
using UnityEngine;

/// <summary>
/// 실제 아이템 슬롯 아이템이 없다면 null
/// 아이템을 추가하면 데이터와 해당 개수를 입력, maxStackSize에 기반한
/// </summary>
[Serializable]
public class InventorySlot
{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int stackSize;

    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;

    /// <summary>
    /// 이미 있는 데이터를 슬롯 생성에 반영할 때
    /// </summary>
    /// <param name="source"></param>
    /// <param name="amount"></param>
    public InventorySlot(InventoryItemData source, int amount)
    {
        itemData = source;
        stackSize = amount;
    }

    /// <summary>
    /// 슬롯만 생성할 때
    /// </summary>
    public InventorySlot()
    {
        itemData = null;
        stackSize = -1;
    }

    public void AssignItem(InventorySlot invSlot)
    {
        if (itemData == invSlot.itemData) AddToStack(invSlot.stackSize);
        else
        {
            itemData = invSlot.itemData;
            stackSize = 0;
            AddToStack(invSlot.stackSize);
        }
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
    /// 슬롯에 개수 할당과 동시에 Stack에 남은 공간이 있는지 확인
    /// </summary>
    /// <param name="amountToAdd">더 할 아이템 양</param>
    /// <param name="amountRemaining">남은 slot을 반환</param>
    /// <returns></returns>
    public bool IsroomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = itemData.MaxStackSize - stackSize;

        return IsroomLeftInStack(amountToAdd);
    }

    public bool IsroomLeftInStack(int amountToAdd)
    {
        if (stackSize + amountToAdd <= itemData.MaxStackSize) return true;

        return false;
    }

    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;

        if(stackSize <= 0)
            ClearSlot();
    }
}
