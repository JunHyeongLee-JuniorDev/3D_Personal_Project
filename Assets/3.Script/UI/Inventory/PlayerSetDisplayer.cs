using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSetDisplayer : MonoBehaviour
{
    private Dictionary<EEquipmentType, InventorySlot_UI> equipments;

    private PlayerHud_UI PlayerHud_UI;
    private InventorySlot_UI[] slots_UI;

    private void Start()
    {
        slots_UI = new InventorySlot_UI[4];
        slots_UI[0] = transform.GetChild(0).GetComponent<InventorySlot_UI>();
        slots_UI[1] = transform.GetChild(1).GetComponent<InventorySlot_UI>();
        slots_UI[2] = transform.GetChild(2).GetComponent<InventorySlot_UI>();
        slots_UI[3] = transform.GetChild(3).GetComponent<InventorySlot_UI>();

        AssignSlot();
    }

    /// <summary>
    /// 메니저에서 불러온 장비 데이터를 불러옴
    /// </summary>
    /// <param name="playerData">플레이어 데이터 내부의 장비 데이터를 불러옴</param>
    public void AssignSlot()
    {
        equipments = new Dictionary<EEquipmentType, InventorySlot_UI>();
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;

        slots_UI[0].Init(_playerData.equipments[(int)EEquipmentType.Weapon]);
        equipments.Add(EEquipmentType.Weapon, slots_UI[0]);

        slots_UI[1].Init(_playerData.equipments[(int)EEquipmentType.Heal]);
        equipments.Add(EEquipmentType.Heal, slots_UI[1]);

        slots_UI[2].Init(_playerData.equipments[(int)EEquipmentType.Mana]);
        equipments.Add(EEquipmentType.Mana, slots_UI[2]);

        slots_UI[3].Init(_playerData.equipments[(int)EEquipmentType.Shield]);
        equipments.Add(EEquipmentType.Shield, slots_UI[3]);

        _playerData.OnWeaponChanged += UpdateSlots;
    }

    public void UpdateSlots()
    {
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;
        if(_playerData.equipments[(int)EEquipmentType.Weapon].StackSize > 0)
            slots_UI[0].UpdateUISlot(_playerData.equipments[(int)EEquipmentType.Weapon]);

        slots_UI[1].UpdateUISlot(_playerData.equipments[(int)EEquipmentType.Heal]);
        slots_UI[2].UpdateUISlot(_playerData.equipments[(int)EEquipmentType.Mana]);

        if (_playerData.equipments[(int)EEquipmentType.Shield].StackSize > 0)
            slots_UI[3].UpdateUISlot(_playerData.equipments[(int)EEquipmentType.Shield]);
    }
}
