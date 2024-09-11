using System;
using System.Collections.Generic;
using System.Linq;
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
        OnInventorySlotChanged += AddSlot;

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    /// <summary>
    /// ���� �κ��丮�� �������� ���� ���ϸ� false�� ��ȯ�ϸ� ����� ����
    /// </summary>
    /// <returns></returns>
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        if (IsContainsItem(itemToAdd, out List<InventorySlot> invSlot))
        {// ���� �������� �κ��丮�� �ִ��� Ȯ��

            foreach (InventorySlot slot in invSlot)
            {
                if (slot.IsroomLeftInStack(amountToAdd))
                {// ������ ������ �߰��� ���� ���϶�� �������� �߰�
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
                // �ƴ϶�� ��� ���� �ڸ��� ã��
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot))
        {// ���� ������ �ִ���
            freeSlot = new InventorySlot(itemToAdd, amountToAdd);
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }

        return false;
    }

    public bool IsContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot)
    {
        invSlot = inventorySlots.Where(slot => slot.ItemData == itemToAdd).ToList();

        //���� �������� ������ �� �� �̻��̶�� true �ƴϸ� false
        return invSlot.Count > 0;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        /* using System.Linq
         * FirstOrDefault()�޼ҵ��
         * 2���� overload
         * �ʱ�ȭ ���� �ʾҴٸ� null ��ȯ
         * �ް����� X = �÷����� ù ��° ��Ҹ� ��ȯ �÷����� ����ٸ� �ش� ��� type�� default(�� ��) ���� ��ȯ
         * �Ű����� Func�� ���� predicate�� �Է� ������ true�� ù ��Ҹ� ��ȯ ���ٸ� �� �� ��ȯ
         * Ŭ������ null�� ��ȯ, int �� 0�� ��ȯ...
         */
        freeSlot = inventorySlots.FirstOrDefault(slot => slot.ItemData == null);
        return freeSlot != null;
    }

    private void AddSlot(InventorySlot slot)
    {

    }

}
