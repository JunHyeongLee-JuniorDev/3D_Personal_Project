using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public PlayerData savePlayerData;

    //��� ������ �����͸� ���� ���𰡰� �ʿ�

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
    public int life;// �����
    public int strength;// ��(�ҵ�)
    public int stamina;// ���׹̳�(����, Ȱ)
    public int intelligence;// ����(����)
}

[Serializable]
public class InventorySaveData
{
    public List<InventorySlot> slots;
    public int wallet;
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