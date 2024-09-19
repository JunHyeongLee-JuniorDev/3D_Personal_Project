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
        savePlayerData.eqiupments = new InventorySlot[savePlayerData.eqiupmentLength];//3칸으로 하드코딩

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
    public readonly int eqiupmentLength = 3;
    /// <summary>
    /// (0 : 무기, 1 : 체력, 2 : 마나)
    /// </summary>
    public InventorySlot[] eqiupments;
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