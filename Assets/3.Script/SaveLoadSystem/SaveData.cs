using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    public GameSaveData()
    {
        savePlayerData = new PlayerData();

        consumeInvData = new InventorySaveData();
        consumeInvData.slots = new List<InventorySlot>();

        equipmentInvData = new InventorySaveData();
        equipmentInvData.slots = new List<InventorySlot>();

        missionInvData = new InventorySaveData();
        missionInvData.slots = new List<InventorySlot>();
    }


    public PlayerData savePlayerData;

    //��� ������ �����͸� ���� ���𰡰� �ʿ�

    public InventorySaveData consumeInvData;
    public InventorySaveData equipmentInvData;
    public InventorySaveData missionInvData;
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
}

[Serializable]
public class InventorySaveData
{
    public List<InventorySlot> slots;
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
}