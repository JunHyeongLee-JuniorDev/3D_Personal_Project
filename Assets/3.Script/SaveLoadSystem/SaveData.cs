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

    //모든 몬스터의 데이터를 담을 무언가가 필요 리스트로 저장 전 클리어 후 다시 add
    public List<monsterSaveData> monsterSaveDatas;

    //화톳불 데이터 리스트 : 데이터의 이름으로 값을 찾음 foreach
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
    public int resolutionIndex; // 해상도
    public int urpAssetsIndex; // urp 에셋 바꾸기
    public bool isFullScreen; // 전체 화면

    public float SFXVolume; // 효과음
    public float masterVolume; // 전체 볼륨
    public float musicVolume; // 음악 볼륨
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
    public int level;// 레벨
    public int lifeStat;// 생명력
    public int strengthStat;// 힘(소드)
    public int staminaStat;// 스테미나(도끼, 활)
    public int manaStat;//(마법)
    public int wallet;//지갑~

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
    /// 처음 게임을 생성하면 생성 무기, 방패는 원래 비어있기에 빈 슬롯할당
    /// </summary>
    /// <param name="healPotion">외부에서 resources폴더에서 할당</param>
    /// <param name="manaPotion">외부에서 resources폴더에서 할당</param>
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
    /// 장비는 착용 소비 아이템은 소비
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
                Debug.Log("이전 무기 stack size : " + equipments[(int)EEquipmentType.Weapon].StackSize);
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
            if (currentPotion.Data.displayName == "체력 포션")
                healHealth(currentPotion.Data.stat);

            else
                healMana(currentPotion.Data.stat);
        }
    }
    /// <summary>
    /// max를 넘는 값이 들어오면 나머지 값은 버림
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
    /// 몬스터의 데미지는 하드코딩 되어 있습니다.
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