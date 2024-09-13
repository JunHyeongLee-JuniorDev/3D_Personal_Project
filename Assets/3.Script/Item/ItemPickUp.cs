using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        switch (itemData.ItemType)
        {
            case EItemType.GEAR:
                if (InventoryManager.instance.InvSys_Gear.AddToInventory(itemData, 1))// �������� �� �ڸ��� �ִٸ�
                {
                    Destroy(gameObject); // ���ٸ� Destroy
                }
                break;

            case EItemType.CONSUME:
                if (InventoryManager.instance.InvSys_Consume.AddToInventory(itemData, 1))// �������� �� �ڸ��� �ִٸ�
                {
                    Destroy(gameObject); // ���ٸ� Destroy
                }
                break;

            case EItemType.MISSION:
                if (InventoryManager.instance.InvSys_mission.AddToInventory(itemData, 1))// �������� �� �ڸ��� �ִٸ�
                {
                    Destroy(gameObject); // ���ٸ� Destroy
                }
                break;

            default:
                Debug.LogError("������ type�� �������� ����");
                break;
        }
    }
}
