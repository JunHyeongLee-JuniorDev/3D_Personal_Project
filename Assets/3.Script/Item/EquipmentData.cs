using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Equipment Item")]
public class EquipmentData : InventoryItemData
{
    //장비 모델
    [SerializeField] private GameObject model;
    public GameObject Model => model;

    //장비 종류
    [SerializeField] private EEquipmentType type;
    public EEquipmentType Type => type;

    //아이템 레벨
    [SerializeField] private int level;
    public int Level => level;

    //아이템 스텟
    [SerializeField] private float stat;
    public float Stat => stat;

    //아이템 강화 상승치
    [SerializeField] private float reinforceStat;
    public float ReinforceStat => reinforceStat;

    //아이템 구매 가격
    [SerializeField] private float buyPrice;
    public float BuyPrice => buyPrice;

    //아이템 강화시 가격 상승치
    [SerializeField] private float reinforceRate;
    public float ReinforceRate => reinforceRate;

    //아이템 강화 가격
    [SerializeField] private float reinforcePrice;
    public float ReinforcePrice => reinforcePrice;

    /// <summary>
    /// 레벨 1, 스텟, 강화 가격 상승
    /// </summary>
    public void Reinforce()
    {
        level += 1;
        stat *= reinforceStat;
        reinforceStat *= reinforceRate;
        reinforceRate *= reinforcePrice;
    }
}
