using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 실제 아이템 인스턴스에 부착하는 컴포넌트 콜라이더에 플레이어 들어오면 아이템 추가 판별
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    public float pickUpRadius = 1f; // Pick Up 반경
    public InventoryItemData itemData;

    private SphereCollider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = pickUpRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; // 플레이어가 아니라면 return

        var inventory = other.GetComponent<InventoryManager>();
        if(inventory == null) return;

        if (inventory.InventorySystem.AddToInventory(itemData, 1))// 아이템이 들어갈 자리가 있다면
        {
            Destroy(gameObject); // 들어갔다면 Destroy
        }
    }
}
