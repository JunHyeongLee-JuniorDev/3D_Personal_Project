using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    GEAR,
    CONSUME,
    MISSION
};

public enum EEquipmentType
{
    AXE,
    SWORD,
    MAGICSTICK,
    BOW,
    SHIELD
};
/// <summary>
/// scriptable obj æ∆¿Ã≈€(ID, Name, Sprite, Max Stack)
/// </summary>
[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    [SerializeField]
    protected string displayName;
    public string DisplayName => displayName;

    [TextArea(4,4)]
    [SerializeField]
    protected string description;
    public string Description => description;

    [SerializeField]
    protected Sprite icon;
    public Sprite Icon => icon;

    [SerializeField]
    protected int maxStackSize;
    public int MaxStackSize => maxStackSize;

    [SerializeField]
    protected EItemType itemType;
    public EItemType ItemType => itemType;

    [SerializeField]
    protected bool isCanSale;
    public bool IsCanSale => isCanSale;

    [SerializeField]
    protected bool isCanDrop;
    public bool IsCanDrop => isCanDrop;
}
