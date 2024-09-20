using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
[CreateAssetMenu(menuName = "Inventory System/Consume Item")]
public class ConsumeItemData : InventoryItemData
{
    [SerializeField]
    private float stat;
    public float Stat => stat;

    [SerializeField]
    private int updatableStack;
    public int UpdatableStack => updatableStack;

    public void ExtendUpdatableStack(int amount)
    {
        if(updatableStack + amount <= maxStackSize)
        updatableStack += amount;
    }
}
