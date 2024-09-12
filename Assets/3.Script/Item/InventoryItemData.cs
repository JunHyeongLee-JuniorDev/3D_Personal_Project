using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    EQUIPMENT,
    MONEYPOUCH,
    WHEAPONOVER,
    POTIONOVER,
    POTION,
    KEY
};

public enum EEquipmentType
{
    WHEAPON,
    HELMET,
    GLOVE,
    CHEST,
    PANTS,
    SHOES,
    SHIELD
};

public enum EWeaponType
{
    AXE,
    SWORD,
    MAGICSTICK,
    BOW
}
/// <summary>
/// scriptable obj æ∆¿Ã≈€(ID, Name, Sprite, Max Stack)
/// </summary>
[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    [SerializeField]
    private int id;
    public int ID => id;

    [SerializeField]
    private string displayName;
    public string DisplayName => displayName;

    [TextArea(4,4)]
    [SerializeField]
    private string description;
    public string Description => description;

    [SerializeField]
    private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField]
    private int maxStackSize;
    public int MaxStackSize => maxStackSize;

    [SerializeField]
    private EItemType itemType;
    public EItemType ItemType => itemType;

    [SerializeField]
    private bool isCanSale;
    public bool IsCanSale => isCanSale;

    [SerializeField]
    private bool isCanDrop;
    public bool IsCanDrop => isCanDrop;
}
