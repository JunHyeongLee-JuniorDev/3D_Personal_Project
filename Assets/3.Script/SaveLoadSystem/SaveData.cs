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
        savePlayerData.eqiupments = new InventorySlot[savePlayerData.eqiupmentLength];//3ĭ���� �ϵ��ڵ�

        for (int i = 0; i < savePlayerData.eqiupments.Length; i++)
        {
            savePlayerData.eqiupments[i] = new InventorySlot();
        }

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
    public readonly int eqiupmentLength = 3;
    /// <summary>
    /// (0 : ����, 1 : ü��, 2 : ����)
    /// </summary>
    public InventorySlot[] eqiupments;
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