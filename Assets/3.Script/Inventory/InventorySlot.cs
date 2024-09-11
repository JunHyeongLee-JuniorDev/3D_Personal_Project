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
    /// �������� ������ �� ������ �����ؾ� �� ��
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
    /// ���� �޼ҵ带 ����Ͽ� ���Գ��� ���ϱⰡ ����
    /// </summary>
    /// <param name="amountToAdd">�� �� ������ ��</param>
    /// <param name="amountRemaining">���� slot�� ��ȯ</param>
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
