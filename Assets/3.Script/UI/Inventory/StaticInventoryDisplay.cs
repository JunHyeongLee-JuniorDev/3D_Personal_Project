using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 inventorysystem을 insperctor에서 참조(플렝이어 인벤토리 싱글톤 필요)하여 인벤토리를 UI에 띄움
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

        else Debug.Log($"{gameObject} 인벤토리가 할당되지 않았습니다.");

        AssignSlot(inventorySystem);
    }

    /// <summary>
    /// 인벤토리 슬롯 초기화 메서드
    /// </summary>
    /// <param name="invToDisplay"> 플레이어 인스턴스의 실제 인벤토리 </param>
    public override void AssignSlot(InventorySystem invToDisplay)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (!slots.Length.Equals(inventorySystem.InventorySize)) Debug.Log($"Iventory slots out of sync on {gameObject}");
        //슬롯의 개수가 플레이어의 인벤토리 총량과 맞지 않을 경우

        for (int i = 0; i < inventorySystem.InventorySize; i++)
        {
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
            //UI Slot에 실제 인벤토리 Slot 값 할당
        }

    }
}
