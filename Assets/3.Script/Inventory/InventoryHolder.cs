using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField]
    private int inventorySize;

    [SerializeField]
    protected InventorySystem inventorySystem;

    public InventorySystem InventorySystem => inventorySystem;

    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequest;

    private void Awake()
    {
        inventorySystem = new InventorySystem(inventorySize);
    }

}
