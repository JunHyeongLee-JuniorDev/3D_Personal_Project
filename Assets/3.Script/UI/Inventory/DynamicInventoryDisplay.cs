using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DynamicInventoryDisplay : MonoBehaviour
{
    [SerializeField]protected PlayerHud_UI playerHud_UI;
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
                slot.Key.Init(updatedSlot); 
                slot.Key.UpdateUISlot(updatedSlot);
            }
        }
    }

    public void SlotLeftClicked(InventorySlot_UI clickedUISlot)
    {
        if (clickedUISlot.AssignedInventorySlot.Data == null) return;

        switch (clickedUISlot.AssignedInventorySlot.Data.itemType)
        {
            case EItemType.CONSUME:
                playerHud_UI.consumeDisplay.UpdateUI(clickedUISlot.AssignedInventorySlot.Data);
                break;

            case EItemType.GEAR:
                playerHud_UI.gearDisplay.UpdateUI(clickedUISlot.AssignedInventorySlot.Data);
                break;

            case EItemType.MISSION:
                playerHud_UI.missionDisplay.UpdateUI(clickedUISlot.AssignedInventorySlot.Data);
                break;
        }
    }
    public void SlotRightClicked(InventorySlot_UI clickedUISlot)
    {
        if (clickedUISlot.AssignedInventorySlot.Data == null) return;
        Managers.Instance.Game.UIGroupStack.Push(playerHud_UI.mouseItemControl.GetComponent<FadeInOut_UI>());
        playerHud_UI.mouseItemControl.gameObject.SetActive(true);
        playerHud_UI.mouseItemControl.UpdateMouseStat(clickedUISlot);
    }

}
