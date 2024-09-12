using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������ �ν��Ͻ��� �����ϴ� ������Ʈ �ݶ��̴��� �÷��̾� ������ ������ �߰� �Ǻ�
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    public float pickUpRadius = 1f; // Pick Up �ݰ�
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
        if (!other.CompareTag("Player")) return; // �÷��̾ �ƴ϶�� return

        var inventory = other.GetComponent<InventoryManager>();
        if(inventory == null) return;

        if (inventory.InventorySystem.AddToInventory(itemData, 1))// �������� �� �ڸ��� �ִٸ�
        {
            Destroy(gameObject); // ���ٸ� Destroy
        }
    }
}
