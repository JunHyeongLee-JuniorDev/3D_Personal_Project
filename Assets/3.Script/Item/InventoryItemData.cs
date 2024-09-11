using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    [SerializeField]
    private int ID;
    public int _ID => ID;

    [SerializeField]
    private string displayName;
    public string _displayName => displayName;

    [TextArea(4,4)]
    [SerializeField]
    private string description;
    public string _description => description;

    [SerializeField]
    private Sprite Icon;
    public Sprite _Icon => Icon;

    [SerializeField]
    private int maxStackSize;
    public int _maxStackSize => maxStackSize;
}
