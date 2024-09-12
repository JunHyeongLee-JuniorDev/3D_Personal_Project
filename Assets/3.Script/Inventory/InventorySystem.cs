using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 실제 인벤토리 상요작용 메서드가 존재, 인벤토리 슬롯 변수 존재
/// </summary>
[System.Serializable]
public class InventorySystem
{
    [SerializeField]
    private List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;

    public int InventorySize => InventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;// 인벤토리 슬롯이 변화 되었다면

    /// <summary>
    /// 생성할 때 inspector에서 설정한 size대로 slot을 할당
    /// </summary>
    /// <param name="size"></param>
    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    /// <summary>
    /// 만약 인벤토리에 아이템을 넣지 못하면 false를 반환하며 담기지 않음
    /// </summary>
    /// <returns></returns>
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        if (IsContainsItem(itemToAdd, out List<InventorySlot> invSlot))
        {// 같은 아이템이 인벤토리에 있는지 확인

            foreach (InventorySlot slot in invSlot)
            {
                if (slot.IsroomLeftInStack(amountToAdd))
                {// 슬롯의 max스택이 추가할 양의 이하라면 아이템을 추가
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
                // 아니라면 계속 남은 자리를 찾음
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot))
        {// 남은 슬롯이 있는지
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }

        return false;
    }

    public bool IsContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot)
    {
        invSlot = inventorySlots.Where(slot => slot.ItemData == itemToAdd).ToList();

        //같은 아이템의 슬롯이 한 개 이상이라면 true 아니면 false
        return invSlot.Count > 0;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        /* using System.Linq
         * FirstOrDefault()메소드란
         * 2개의 overload
         * 초기화 되지 않았다면 null 반환
         * 메개변수 X = 컬렉션의 첫 번째 요소를 반환 컬렉션이 비었다면 해당 요소 type의 default(빈 값) 값을 반환
         * 매개변수 Func로 만든 predicate로 입력 조건이 true인 첫 요소를 반환 없다면 빈 값 반환
         * 클래스는 null을 반환, int 는 0을 반환...
         * 클래스는 null을 반환, int 는 0을 반환...
         */
        freeSlot = inventorySlots.FirstOrDefault(slot => slot.ItemData == null);

        return freeSlot != null;
    }
}