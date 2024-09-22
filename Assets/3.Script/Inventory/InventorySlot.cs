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
    [SerializeField] private ItemData data;
    [SerializeField] private int stackSize;

    public ItemData Data => data;
    public int StackSize => stackSize;

    /// <summary>
    /// �̹� �ִ� �����͸� ���� ������ �ݿ��� ��
    /// </summary>
    /// <param name="source"></param>
    /// <param name="amount"></param>
    public InventorySlot(ItemData source, int amount)
    {
        data = source;
        stackSize = amount;
    }

    /// <summary>
    /// ���Ը� ������ ��
    /// </summary>
    public InventorySlot()
    {
        data = null;
        stackSize = -1;
    }

    public void AssignItem(InventorySlot invSlot)
    {
        if (data == invSlot.data) AddToStack(invSlot.stackSize);
        else
        {
            data = invSlot.data;
            stackSize = 0;
            AddToStack(invSlot.stackSize);
        }
    }

    public void ClearSlot()
    {
        data = null;
        stackSize = -1;
    }

    public void UpdateInventorySlot(ItemData Data, int amount)
    {
        data = Data;
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
        amountRemaining = data.maxStackSize - stackSize;

        return IsroomLeftInStack(amountToAdd);
    }

    public bool IsroomLeftInStack(int amountToAdd)
    {
        if (stackSize + amountToAdd <= data.maxStackSize) return true;

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
