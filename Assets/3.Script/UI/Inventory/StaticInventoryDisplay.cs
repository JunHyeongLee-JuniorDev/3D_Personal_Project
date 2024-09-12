using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾��� inventorysystem�� insperctor���� ����(�÷��̾� �κ��丮 �̱��� �ʿ�)�Ͽ� �κ��丮�� UI�� ���
/// </summary>

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlot_UI[] slots;

    protected override void Start()
    {
        base.Start();

        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.InventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }

        else Debug.Log($"{gameObject} �κ��丮�� �Ҵ���� �ʾҽ��ϴ�.");

        AssignSlot(inventorySystem);
    }

    /// <summary>
    /// �κ��丮 ���� �ʱ�ȭ �޼���
    /// </summary>
    /// <param name="invToDisplay"> �÷��̾� �ν��Ͻ��� ���� �κ��丮 </param>
    public override void AssignSlot(InventorySystem invToDisplay)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (!slots.Length.Equals(inventorySystem.InventorySize)) Debug.Log($"Iventory slots out of sync on {gameObject}");
        //������ ������ �÷��̾��� �κ��丮 �ѷ��� ���� ���� ���

        for (int i = 0; i < inventorySystem.InventorySize; i++)
        {
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
            //UI Slot�� ���� �κ��丮 Slot �� �Ҵ�
        }

    }
}
