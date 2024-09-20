using System;
using System.Xml;
using UnityEngine;

/// <summary>
/// ���� ������ ���� �������� ���ٸ� null
/// �������� �߰��ϸ� �����Ϳ� �ش� ������ �Է�, maxStackSize�� �����
/// </summary>
[Serializable]
public class InventorySlot
{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int stackSize;

    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;

    /// <summary>
    /// �̹� �ִ� �����͸� ���� ������ �ݿ��� ��
    /// </summary>
    /// <param name="source"></param>
    /// <param name="amount"></param>
    public InventorySlot(InventoryItemData source, int amount)
    {
        itemData = source;
        stackSize = amount;
    }

    /// <summary>
    /// ���Ը� ������ ��
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
    /// ���Կ� ���� �Ҵ�� ���ÿ� Stack�� ���� ������ �ִ��� Ȯ��
    /// </summary>
    /// <param name="amountToAdd">�� �� ������ ��</param>
    /// <param name="amountRemaining">���� slot�� ��ȯ</param>
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
