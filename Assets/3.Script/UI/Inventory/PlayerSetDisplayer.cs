using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetDisplayer : MonoBehaviour
{
    private Dictionary<InventorySlot, InventorySlot_UI> equipments;

    private PlayerHud_UI PlayerHud_UI;
    private InventorySlot_UI[] slots_UI;
    private Image currentPotionFrame;

    private void Start()
    {
        slots_UI = new InventorySlot_UI[4];
        slots_UI[0] = transform.GetChild(0).GetComponent<InventorySlot_UI>();
        slots_UI[1] = transform.GetChild(1).GetComponent<InventorySlot_UI>();
        slots_UI[2] = transform.GetChild(2).GetComponent<InventorySlot_UI>();
        slots_UI[3] = transform.GetChild(3).GetComponent<InventorySlot_UI>();
        currentPotionFrame = transform.GetChild(4).GetComponent<Image>();
        PlayerHud_UI = GetComponentInParent<PlayerHud_UI>();
        AssignSlot();
        OnWheelChaged();

        Managers.Instance.Inventory.PlayerData.OnWeaponChanged.AddListener(UpdateSlots);
        Managers.Instance.Inventory.PlayerData.OnChangeHealPotion.AddListener(OnWheelChaged);
    }

    /// <summary>
    /// 메니저에서 불러온 장비 데이터를 불러옴
    /// </summary>
    /// <param name="playerData">플레이어 데이터 내부의 장비 데이터를 불러옴</param>
    public void AssignSlot()
    {
        equipments = new Dictionary<InventorySlot, InventorySlot_UI>();
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;

        slots_UI[0].Init(_playerData.equipments[(int)EEquipmentType.Weapon]);
        equipments.Add(_playerData.equipments[(int)EEquipmentType.Weapon], slots_UI[0]);

        slots_UI[1].Init(_playerData.equipments[(int)EEquipmentType.Heal]);
        equipments.Add(_playerData.equipments[(int)EEquipmentType.Heal], slots_UI[1]);

        slots_UI[2].Init(_playerData.equipments[(int)EEquipmentType.Mana]);
        equipments.Add(_playerData.equipments[(int)EEquipmentType.Mana], slots_UI[2]);

        slots_UI[3].Init(_playerData.equipments[(int)EEquipmentType.Shield]);
        equipments.Add(_playerData.equipments[(int)EEquipmentType.Shield], slots_UI[3]);

        _playerData.OnWeaponChanged.AddListener(UpdateSlots);
    }

    public void UpdateSlots()
    {
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;

        for (int i = 0; i < equipments.Count; i++)
        {
            equipments[_playerData.equipments[i]].Init(_playerData.equipments[i]);
        }

    }

    public void OnLeftClicked(InventorySlot_UI slot_UI)
    {
        Debug.Log("여긴 눌리니?");
        if (slot_UI.AssignedInventorySlot.StackSize < 0) return;
        PlayerHud_UI.gearInv.InventorySystem.AddToInventory(slot_UI.AssignedInventorySlot.Data, slot_UI.AssignedInventorySlot.StackSize);


        foreach (var item in equipments)
        {
            if (item.Value.Equals(slot_UI))
            {
                item.Key.ClearSlot();
            }
        }

        slot_UI.ClearSlot();
        Managers.Instance.Inventory.PlayerData.OnWeaponChanged?.Invoke();
    }

    public void OnWheelChaged()
    {
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;

        if (_playerData.currentPotion.Data.displayName == "체력 포션")
        {
            Debug.Log("체력 물약");
            currentPotionFrame.rectTransform.anchoredPosition =
                equipments[_playerData.equipments[(int)EEquipmentType.Heal]].GetComponent<RectTransform>().anchoredPosition;
        }

        else
        {
            Debug.Log("마나 물약");
            currentPotionFrame.rectTransform.anchoredPosition =
                equipments[_playerData.equipments[(int)EEquipmentType.Mana]].GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
