using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EEquipmentType
{
    Shield,
    Weapon,
    Heal,
    Mana,
    Length
};

[Serializable]
public class GameSaveData
{
    public GameSaveData()
    {
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
    public InventorySlot[] equipments;

    public UnityAction OnWeaponChanged;

    /// <summary>
    /// ó�� ������ �����ϸ� ���� ����, ���д� ���� ����ֱ⿡ �� �����Ҵ�
    /// </summary>
    /// <param name="healPotion">�ܺο��� resources�������� �Ҵ�</param>
    /// <param name="manaPotion">�ܺο��� resources�������� �Ҵ�</param>
    public PlayerData(InventorySlot healPotion, InventorySlot manaPotion)
    {
        equipments = new InventorySlot[(int)EEquipmentType.Length];
        equipments[(int)EEquipmentType.Shield] = new InventorySlot();
        equipments[(int)EEquipmentType.Weapon] = new InventorySlot();
        equipments[(int)EEquipmentType.Heal] = healPotion;
        equipments[(int)EEquipmentType.Mana] = manaPotion;
    }

    /// <summary>
    /// ���� ���� �Һ� �������� �Һ�
    /// </summary>
    /// <param name="newEquipment"></param>
    /// <param name="amount"></param>
    public void UpdateWeaponAndShield(InventorySlot_UI newEquipment, int amount)
    {
        if (newEquipment.AssignedInventorySlot.Data.itemType.Equals(EItemType.GEAR))
        {
            if((newEquipment.AssignedInventorySlot.Data.weaponType.Equals(EWeaponType.SHIELD)))
            {
                equipments[(int)EEquipmentType.Shield].UpdateInventorySlot(
                                newEquipment.AssignedInventorySlot.Data, amount);
                return;
            }

            equipments[(int)EEquipmentType.Weapon].UpdateInventorySlot(
                                                newEquipment.AssignedInventorySlot.Data, amount);
            return;
        }
    }

    public void refillPotion()
    {
        ItemData _itemData = equipments[(int)EEquipmentType.Heal].Data;
        equipments[(int)EEquipmentType.Heal].AddToStack(_itemData.updatableStack);

        _itemData = equipments[(int)EEquipmentType.Mana].Data;
        equipments[(int)EEquipmentType.Mana].AddToStack(_itemData.updatableStack);
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