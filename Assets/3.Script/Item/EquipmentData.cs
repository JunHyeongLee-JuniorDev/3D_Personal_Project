using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Equipment Item")]
public class EquipmentData : InventoryItemData
{
    [SerializeField]
    private EEquipmentType type;
    public EEquipmentType Type => type;

    [SerializeField]
    private int level;
    public int Level => level;

    [SerializeField]
    private float state;
    public float State => state;

    [SerializeField]
    private float reinforceState;

    public float ReinforceState => reinforceState;

    [SerializeField]
    private float buyPrice;
    public float BuyPrice => buyPrice;

    [SerializeField]
    private float priceRate;
    public float PriceRate => priceRate;

    [SerializeField]
    private float reinforcePrice;
    public float ReinforcePrice => reinforcePrice;

    /// <summary>
    /// ���� 1, ����, ��ȭ ���� ���
    /// </summary>
    public void reinforce()
    {
        level += 1;
        state *= reinforceState;
        reinforceState *= priceRate;
    }
}
