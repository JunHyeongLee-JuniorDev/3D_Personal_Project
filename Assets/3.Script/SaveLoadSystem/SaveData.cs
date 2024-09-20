using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GameSaveData
{
    public GameSaveData()
    {
        savePlayerData = new PlayerData();

        consumeInvData = new InventorySystem(consumeInvSize);
        equipmentInvData = new InventorySystem(equipmentInvSize);
        missionInvData = new InventorySystem(missionInvSize);

        monsterSaveDatas = new List<monsterSaveData>();
    }


    public PlayerData savePlayerData;

    //��� ������ �����͸� ���� ���𰡰� �ʿ� ����Ʈ�� ���� �� Ŭ���� �� �ٽ� add
    public List<monsterSaveData> monsterSaveDatas;

    public InventorySystem consumeInvData;
    public InventorySystem equipmentInvData;
    public InventorySystem missionInvData;

    private readonly int consumeInvSize = 20;
    private readonly int equipmentInvSize = 20;
    private readonly int missionInvSize = 20;
}

[Serializable]
public class SettingSaveData
{
    public int resolutionIndex; // �ػ�
    public int urpAssetsIndex; // urp ���� �ٲٱ�
    public bool isFullScreen; // ��ü ȭ��

    public float SFXVolume; // ȿ����
    public float masterVolume; // ��ü ����
    public float musicVolume; // ���� ����
}

/// <summary>
/// PlayerSaveData(health, mana, postion, etc...)
/// </summary>
[Serializable]
public class PlayerData
{
    //User Name
    public string name;

    //Physics
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    //Health
    public float currentHealth;
    public float maxHealth;

    //Stat
    public int level;// ����
    public int lifeStat;// �����
    public int strengthStat;// ��(�ҵ�)
    public int staminaStat;// ���׹̳�(����, Ȱ)
    public int intelligenceStat;// ����(����)
    public int wallet;//����~

    //Gears
    public readonly int eqiupmentLength = 4;
    /// <summary>
    /// (0 : ����, 1 : ü��, 2 : ����, 3 : ����)
    /// </summary>
    public Dictionary<string, InventorySlot> equipments;

    public UnityAction<InventorySlot> OnWeaponChanged;

    public PlayerData()
    {
        equipments = new Dictionary<string, InventorySlot>();

        Debug.Log("�ٵ� ���� �翬�� �ȵ����°� �ƴѰ�?");
        equipments.Add("Weapon", new InventorySlot());
        equipments.Add("Shield", new InventorySlot());
    }

    /// <summary>
    /// ���� ���� �Һ� �������� �Һ�
    /// </summary>
    /// <param name="newEquipment"></param>
    /// <param name="amount"></param>
    public void UseEquipments(InventoryItemData newEquipment, int amount)
    {
        if (newEquipment.ItemType.Equals(EItemType.GEAR))
        {
            if((newEquipment as EquipmentData).EquipmentType.Equals(EEquipmentType.SHIELD))
            {
                equipments["Shield"].UpdateInventorySlot(newEquipment, amount);
                return;
            }
            equipments["Weapon"].UpdateInventorySlot(newEquipment, amount);
            return;
        }

        switch(newEquipment.DisplayName)
        {
            case "ü�� ����":
                if (equipments["Heal"].StackSize > 0)
                    equipments["Heal"].RemoveFromStack(amount);
                break;

            case "���� ����":
                if (equipments["Mana"].StackSize > 0)
                    equipments["Mana"].RemoveFromStack(amount);
                break;
        }
    }

    public void refillPotion()
    {
        ConsumeItemData _itemData = equipments["Heal"].ItemData as ConsumeItemData;
        equipments["Heal"].AddToStack(_itemData.UpdatableStack);

        _itemData = equipments["Mana"].ItemData as ConsumeItemData;
        equipments["Mana"].AddToStack(_itemData.UpdatableStack);
    }
}

    /// <summary>
    /// ������ �������� �ϵ��ڵ� �Ǿ� �ֽ��ϴ�.
    /// </summary>
    [Serializable]
public class monsterSaveData
{
    //Physics
    public Vector3 monsterPosition;
    public Quaternion monsterRotation;

    //Health
    public float currentHealth;
    public float maxHealth;
}