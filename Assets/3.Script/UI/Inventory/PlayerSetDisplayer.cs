using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSetDisplayer : MonoBehaviour
{
    private Dictionary<string, InventorySlot_UI> equipments;

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
        equipments = new Dictionary<string, InventorySlot_UI>();


        InventorySlot_UI _uiSlot = transform.GetChild(0).GetComponent<InventorySlot_UI>();
        _uiSlot.Init(playerData.equipments["Weapon"]);
        equipments.Add("Weapon", _uiSlot);

        _uiSlot = transform.GetChild(1).GetComponent<InventorySlot_UI>();
        _uiSlot.Init(playerData.equipments["Heal"]);
        equipments.Add("Heal", _uiSlot);

        _uiSlot = transform.GetChild(2).GetComponent<InventorySlot_UI>();
        _uiSlot.Init(playerData.equipments["Mana"]);
        equipments.Add("Mana", _uiSlot);

        playerData.OnWeaponChanged += SwitchWeapon;
    }

    private void SwitchWeapon(InventorySlot inventorySlot)
    {
        if (inventorySlot.ItemData.ItemType.Equals(EItemType.GEAR))
        {
            equipments["Weapon"].UpdateUISlot(inventorySlot);
            return;
        }
    }
}
