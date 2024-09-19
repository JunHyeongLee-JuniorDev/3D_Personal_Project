using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Equipment Item")]
public class EquipmentData : InventoryItemData
{
    //��� ��
    [SerializeField] private GameObject model;
    public GameObject Model => model;

    //��� ����
    [SerializeField] private EEquipmentType type;
    public EEquipmentType Type => type;

    //������ ����
    [SerializeField] private int level;
    public int Level => level;

    //������ ����
    [SerializeField] private float stat;
    public float Stat => stat;

    //������ ��ȭ ���ġ
    [SerializeField] private float reinforceStat;
    public float ReinforceStat => reinforceStat;

    //������ ���� ����
    [SerializeField] private float buyPrice;
    public float BuyPrice => buyPrice;

    //������ ��ȭ�� ���� ���ġ
    [SerializeField] private float reinforceRate;
    public float ReinforceRate => reinforceRate;

    //������ ��ȭ ����
    [SerializeField] private float reinforcePrice;
    public float ReinforcePrice => reinforcePrice;

    /// <summary>
    /// ���� 1, ����, ��ȭ ���� ���
    /// </summary>
    public void Reinforce()
    {
        level += 1;
        stat *= reinforceStat;
        reinforceStat *= reinforceRate;
        reinforceRate *= reinforcePrice;
    }
}
