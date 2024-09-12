using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// 실제 플레이어가 갖고 있는 인벤토리
/// </summary>
[Serializable]
public class InventoryManager : MonoBehaviour // IInitManager
{
    [SerializeField]
    private int inventorySize;// 인벤토리 리스트의 크기를 정함

    [SerializeField]
    protected InventorySystem inventorySystem; // 인벤토리

    public InventorySystem InventorySystem => inventorySystem;

    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequest;// 인벤토리가 UI와 상호작용이 있을 때
    public void Awake()
    {
        inventorySystem = new InventorySystem(inventorySize);
    }
}
