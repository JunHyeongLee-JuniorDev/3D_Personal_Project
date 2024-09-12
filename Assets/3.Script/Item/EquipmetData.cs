using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Weapon Item")]
public class EquipmetData : InventoryItemData
{
    [SerializeField]
    private EWeaponType weaponType;
    public EWeaponType WeaponType => weaponType;

    [SerializeField]
    private float damage;
    public float Damage => damage;
}
