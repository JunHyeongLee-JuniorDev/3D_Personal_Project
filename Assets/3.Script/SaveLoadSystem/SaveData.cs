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

    //모든 몬스터의 데이터를 담을 무언가가 필요 리스트로 저장 전 클리어 후 다시 add
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
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    //Health
    public float currentHealth;
    public float maxHealth;

    //Stat
    public int level;// 레벨
    public int lifeStat;// 생명력
    public int strengthStat;// 힘(소드)
    public int staminaStat;// 스테미나(도끼, 활)
    public int intelligenceStat;// 지능(마법)
    public int wallet;//지갑~

    //Gears
    public readonly int eqiupmentLength = 4;
    public InventorySlot[] equipments;

    public UnityAction OnWeaponChanged;

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
    /// 몬스터의 데미지는 하드코딩 되어 있습니다.
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