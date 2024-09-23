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
                Debug.LogWarning("�Ҵ���� ���� �κ��丮 type");
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
    /// backend�� �ִ� ���� Slot�� ���� UI Slot�� ����
    /// </summary>
    /// <param name="updatedSlot"> ���� ���� ������ ������ ���� �������� �ִٸ� UI�� ����</param>
    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in slotDictionary)
        {
            if (slot.Value.Equals(updatedSlot)) // ���� UI�� �ִ� �����۰� ���� �������̶�� �߰�
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
