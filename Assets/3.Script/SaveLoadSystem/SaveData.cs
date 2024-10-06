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
        bornFireDatas = new List<BornFireData>();
        
        bornFireDatas.Add(new BornFireData("BrigeFire"));
        bornFireDatas.Add(new BornFireData("HallWayFire"));
    }


    public PlayerData savePlayerData;

    //��� ������ �����͸� ���� ���𰡰� �ʿ� ����Ʈ�� ���� �� Ŭ���� �� �ٽ� add
    public List<monsterSaveData> monsterSaveDatas;

    //ȭ��� ������ ����Ʈ : �������� �̸����� ���� ã�� foreach
    public List<BornFireData> bornFireDatas;

    public InventorySystem consumeInvData;
    public InventorySystem equipmentInvData;
    public InventorySystem missionInvData;
    public bool isFirstDoorOpened;
    public bool isSecondDoorOpened;
    public bool isBossDoorOpened;

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
    public Vector3 spawnPosition;
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    //Status
    public float currentHealth;
    public float maxHealth;
    public float currentMana;
    public float maxMana;
    public float currentStamina;
    public float maxStamina;
    public float weaponDamage;
    public float magicDamage;

    //Stat
    public int level;// ����
    public int lifeStat;// �����
    public int strengthStat;// ��(�ҵ�)
    public int staminaStat;// ���׹̳�(����, Ȱ)
    public int manaStat;//(����)
    public int wallet;//����~

    //dropped money
    public int droppedMoney;
    public Vector3 droppedMoneyPos;

    //Gears
    public readonly int eqiupmentLength = 4;
    public InventorySlot[] equipments;
    public InventorySlot currentPotion;

    public UnityEvent OnWeaponChanged;
    public UnityEvent OnChangeHealPotion;
    public UnityEvent OnRefillStatus;
    public UnityEvent OnReduceStatus;

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
        currentPotion = healPotion;
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
                if (equipments[(int)EEquipmentType.Shield].StackSize > 0)
                {
                    Managers.Instance.Inventory.InvSys_Gear.AddToInventory(equipments[(int)EEquipmentType.Shield].Data,
                                                                           equipments[(int)EEquipmentType.Shield].StackSize);
                }

                equipments[(int)EEquipmentType.Shield].UpdateInventorySlot(
                                newEquipment.AssignedInventorySlot.Data, amount);
                return;
            }

            if (equipments[(int)EEquipmentType.Weapon].StackSize > 0)
            {
                Debug.Log("���� ���� stack size : " + equipments[(int)EEquipmentType.Weapon].StackSize);
                Managers.Instance.Inventory.InvSys_Gear.AddToInventory(equipments[(int)EEquipmentType.Weapon].Data,
                                                                       equipments[(int)EEquipmentType.Weapon].StackSize);
            }

            equipments[(int)EEquipmentType.Weapon].UpdateInventorySlot(
                                                newEquipment.AssignedInventorySlot.Data, amount);
            return;
        }
    }

    public void refillPotion()
    {
        InventorySlot _item = equipments[(int)EEquipmentType.Heal];
        _item.RemoveFromStack(_item.StackSize);
        _item.AddToStack(_item.Data.updatableStack);

        _item = equipments[(int)EEquipmentType.Mana];
        _item.RemoveFromStack(_item.StackSize);
        _item.AddToStack(_item.Data.updatableStack);
    }

    public void DrinkPotion()
    {
        if (currentPotion.StackSize > 0)
        {
            currentPotion.RemoveFromStack(1);
            if (currentPotion.Data.displayName == "ü�� ����")
                healHealth(currentPotion.Data.stat);

            else
                healMana(currentPotion.Data.stat);
        }
    }
    /// <summary>
    /// max�� �Ѵ� ���� ������ ������ ���� ����
    /// </summary>
    /// <param name="addValue"></param>
    private void healHealth(float addValue)
    {
        if(currentHealth + addValue >= maxHealth)
            currentHealth = maxHealth;

        else
            currentHealth += addValue;
    }
    private void healMana(float addValue)
    {
        if (currentMana + addValue >= maxHealth)
            currentMana = maxHealth;

        else
            currentMana += addValue;
    }
    public void ChangePotion()
    {
        if (currentPotion.Equals(equipments[(int)EEquipmentType.Heal]))
            currentPotion = equipments[(int)EEquipmentType.Mana];

        else
            currentPotion = equipments[(int)EEquipmentType.Heal];
    }
}

    /// <summary>
    /// ������ �������� �ϵ��ڵ� �Ǿ� �ֽ��ϴ�.
    /// </summary>
    [Serializable]
public class monsterSaveData
{
    public int monsterID;

    //Health
    public float currentHealth;
    public float maxHealth;

    public bool isBoss;
    public bool isDead;

    public Vector3 defaultSpawnPos;
}
[Serializable]
public class BornFireData
{
    public string fireName;
    public bool isOn;

    public BornFireData(string fireName)
    {
        this.fireName = fireName;
        isOn = false;
    }
}