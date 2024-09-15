using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public PlayerData savePlayerData;

    //모든 몬스터의 데이터를 담을 무언가가 필요

    public InventorySaveData consumeInvData;
    public InventorySaveData equipmentInvData;
    public InventorySaveData missionInvData;
}

[Serializable]
public class PlayerData
{
    //Physics
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    //Health
    public float currentHealth;
    public float maxHealth;

    //Stat
    public int life;// 생명력
    public int strength;// 힘(소드)
    public int stamina;// 스테미나(도끼, 활)
    public int intelligence;// 지능(마법)
}

[Serializable]
public class InventorySaveData
{
    public List<InventorySlot> slots;
    public int wallet;
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
}