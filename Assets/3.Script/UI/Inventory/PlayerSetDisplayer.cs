using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSetDisplayer : MonoBehaviour
{
    private Dictionary<EEquipmentType, InventorySlot_UI> equipments;

    private PlayerHud_UI PlayerHud_UI;

    private void Start()
    {
        AssignSlot(Managers.Instance.Inventory.PlayerData);
    }

    /// <summary>
    /// 메니저에서 불러온 장비 데이터를 불러옴
    /// </summary>
    /// <param name="playerData">플레이어 데이터 내부의 장비 데이터를 불러옴</param>
    public void AssignSlot(PlayerData playerData)
    {
        equipments = new Dictionary<EEquipmentType, InventorySlot_UI>();


        InventorySlot_UI _uiSlot = transform.GetChild(0).GetComponent<InventorySlot_UI>();
        _uiSlot.Init(playerData.equipments[(int)EEquipmentType.Weapon]);
        equipments.Add(EEquipmentType.Weapon, _uiSlot);

        _uiSlot = transform.GetChild(1).GetComponent<InventorySlot_UI>();
        _uiSlot.Init(playerData.equipments[(int)EEquipmentType.Heal]);
        equipments.Add(EEquipmentType.Heal, _uiSlot);

        _uiSlot = transform.GetChild(2).GetComponent<InventorySlot_UI>();
        _uiSlot.Init(playerData.equipments[(int)EEquipmentType.Mana]);
        equipments.Add(EEquipmentType.Mana, _uiSlot);

        _uiSlot = transform.GetChild(3).GetComponent<InventorySlot_UI>();
        _uiSlot.Init(playerData.equipments[(int)EEquipmentType.Shield]);
        equipments.Add(EEquipmentType.Shield, _uiSlot);

        playerData.OnWeaponChanged += SwitchWeaponOrShield;
    }

    private void SwitchWeaponOrShield(InventorySlot inventorySlot)
    {
        if ((inventorySlot.ItemData as WeaponData).WeaponType.Equals(EEquipmentType.Weapon))
        {
            equipments[EEquipmentType.Weapon].UpdateUISlot(inventorySlot);
            return;
        }

        else if ((inventorySlot.ItemData as WeaponData).WeaponType.Equals(EEquipmentType.Shield))
        {
            equipments[EEquipmentType.Shield].UpdateUISlot(inventorySlot);
            return;
        }
    }
}
