using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaveData
{
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
    //Physics
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    //Health
    public float currentHealth;
    public float maxHealth;

    //Stat
    public int level;// ����
    public int life;// �����
    public int strength;// ��(�ҵ�)
    public int stamina;// ���׹̳�(����, Ȱ)
    public int intelligence;// ����(����)
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