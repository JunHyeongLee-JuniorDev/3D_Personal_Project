using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// ���� �÷��̾ ���� �ִ� �κ��丮
/// </summary>
[Serializable]
public class InventoryManager : MonoBehaviour // IInitManager
{
    public static InventoryManager instance = null; // For debug

    [SerializeField]
    private int gearInvSize;// �κ��丮 ����Ʈ�� ũ�⸦ ����
    [SerializeField]
    private int consumeInvSize;// �κ��丮 ����Ʈ�� ũ�⸦ ����
    [SerializeField]
    private int missionInvSize;// �κ��丮 ����Ʈ�� ũ�⸦ ����


    [SerializeField]
    private InventorySystem invSys_Gear; // ��� �κ�
    [SerializeField]
    private InventorySystem invSys_Consume; // �Һ� �κ�
    [SerializeField]
    private InventorySystem invSys_mission; // �̼� �κ�

    public InventorySystem InvSys_Gear => invSys_Gear;
    public InventorySystem InvSys_Consume => invSys_Consume;
    public InventorySystem InvSys_mission => invSys_mission;

    public int wallet;

    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequest;// �κ��丮�� UI�� ��ȣ�ۿ��� ���� ��
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            invSys_Gear = new InventorySystem(gearInvSize);
            invSys_Consume = new InventorySystem(consumeInvSize);
            invSys_mission = new InventorySystem(missionInvSize);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
