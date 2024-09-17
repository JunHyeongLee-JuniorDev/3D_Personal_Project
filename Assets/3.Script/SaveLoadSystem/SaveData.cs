using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    public PlayerData savePlayerData;

    //모든 몬스터의 데이터를 담을 무언가가 필요

    public InventorySaveData consumeInvData;
    public InventorySaveData equipmentInvData;
    public InventorySaveData missionInvData;
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
    //Physics
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    //Health
    public float currentHealth;
    public float maxHealth;

    //Stat
    public int level;// 레벨
    public int life;// 생명력
    public int strength;// 힘(소드)
    public int stamina;// 스테미나(도끼, 활)
    public int intelligence;// 지능(마법)
    public int wallet;//지갑~
}

[Serializable]
public class InventorySaveData
{
    public List<InventorySlot> slots;
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