using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DynamicInventoryDisplay : MonoBehaviour
{
    protected PlayerHud_UI playerHud_UI;
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;
    [SerializeField]protected Transform viewerTransform;
    protected InventorySlot_UI slotPrefab;

    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    [field : SerializeField]
    public InventoryType invType_UI { get; private set; }

    public enum InventoryType
    { 
        Equipment,
        Mission,
        Consume
    };

    protected virtual void Start()
    {
        playerHud_UI = GetComponentInParent<PlayerHud_UI>();
        slotPrefab = Resources.Load<InventorySlot_UI>("Prefabs/InventorySlot_UI");

        switch (invType_UI)
        {
            case InventoryType.Equipment:
                inventorySystem = Managers.Instance.Inventory.InvSys_Gear;
                break;
            case InventoryType.Mission:
                inventorySystem = Managers.Instance.Inventory.InvSys_mission;
                break;
            case InventoryType.Consume:
                inventorySystem = Managers.Instance.Inventory.InvSys_Consume;
                break;
            default:
                Debug.LogWarning("할당되지 않은 인벤토리 type");
                break;
        }
        inventorySystem.OnInventorySlotChanged += UpdateSlot;
        AssignSlot(inventorySystem);
    }

    public virtual void AssignSlot(InventorySystem invToDisplay)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        for (int i = 0; i < invToDisplay.InventorySize; i++)
        {
            var slot_UI = Instantiate(slotPrefab, viewerTransform);
            slotDictionary.Add(slot_UI, invToDisplay.InventorySlots[i]);
            slot_UI.Init(invToDisplay.InventorySlots[i]);
        }
    }

    /// <summary>
    /// backend에 있는 실제 Slot의 값을 UI Slot에 갱신
    /// </summary>
    /// <param name="updatedSlot"> 새로 받은 아이템 슬롯을 같은 아이템이 있다면 UI에 갱신</param>
    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in slotDictionary)
        {
            if (slot.Value.Equals(updatedSlot)) // 만약 UI에 있는 아이템과 같은 아이템이라면 추가
            {
                slot.Key.UpdateUISlot(updatedSlot);
            }
        }
    }


    public void SlotLeftClicked(InventorySlot_UI clickedUISlot)
    {
        if (clickedUISlot.AssignedInventorySlot.ItemData == null) return;

        switch (clickedUISlot.AssignedInventorySlot.ItemData.ItemType)
        {
            case EItemType.CONSUME:
                Debug.Log("설명 로직 필요");
                break;

            case EItemType.GEAR:
                Debug.Log("설명 로직 필요");
                break;

            case EItemType.MISSION:
                Debug.Log("설명 로직 필요");
                break;
        }
    }
    public void SlotRightClicked(InventorySlot_UI clickedUISlot)
    {
        if (clickedUISlot.AssignedInventorySlot.ItemData == null) return;

        switch (invType_UI)
        {
            case InventoryType.Equipment:
                Debug.Log("우클릭 UI 창 띄우기.");
                break;
            case InventoryType.Mission:
                Debug.Log("우클릭 UI 창 띄우기.");
                break;
            case InventoryType.Consume:
                Debug.Log("우클릭 UI 창 띄우기.");
                break;
            default:
                Debug.LogWarning("할당되지 않은 인벤토리 type");
                break;
        }
    }

}
