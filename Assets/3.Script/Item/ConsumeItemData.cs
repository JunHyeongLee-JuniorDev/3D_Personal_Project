using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Inventory System/Consume Item")]
public class ConsumeItemData : InventoryItemData
{
    [SerializeField]
    private float stat;

    [SerializeField]
    private float Stat => stat;
}
