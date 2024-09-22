using System;
using UnityEngine;

public enum EItemType
{
    None,
    GEAR,
    CONSUME,
    MISSION
};

public enum EWeaponType
{
    None,
    AXE,
    SWORD,
    MAGIC,
    SHIELD
};

[Serializable]
public class ItemData
{
    public string displayName;
    public string description;
    public string effectString;
    public int maxStackSize;
    public EItemType itemType;
    public bool isCanSale;
    public bool isCanDrop;
    public float stat;
    public int updatableStack;
    public EWeaponType weaponType;
    public int level;
    public float reinforceStat;
    public float reinforcePrice;

    public void Reinforce()
    {
        level += 1;
        stat *= reinforceStat;
        reinforcePrice *= 1.2f;
    }

    public void ExtendUpdatableStack(int amount)
    {
        updatableStack += amount;
    }
}

[Serializable]
public class ItemResouce
{
    [field: SerializeField] public string itemName { get; private set; }
    [field: SerializeField] public Sprite itemIcon { get; private set; }
    [field: SerializeField] public GameObject itemModel { get; private set; }
}