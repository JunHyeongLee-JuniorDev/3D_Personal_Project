using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// ���� �÷��̾ ���� �ִ� �κ��丮
/// </summary>
[Serializable]
public class InventoryManager : MonoBehaviour // IInitManager
{
    [SerializeField]
    private int inventorySize;// �κ��丮 ����Ʈ�� ũ�⸦ ����

    [SerializeField]
    protected InventorySystem inventorySystem; // �κ��丮

    public InventorySystem InventorySystem => inventorySystem;

    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequest;// �κ��丮�� UI�� ��ȣ�ۿ��� ���� ��
    public void Awake()
    {
        inventorySystem = new InventorySystem(inventorySize);
    }
}
