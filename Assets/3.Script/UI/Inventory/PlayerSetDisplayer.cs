using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSetDisplayer : MonoBehaviour
{
    [SerializeField] private InventorySlot_UI[] equipments;

    private PlayerHud_UI PlayerHud_UI;


    private Dictionary<InventorySlot, InventorySlot_UI> equipmentsDictionary = new Dictionary<InventorySlot, InventorySlot_UI>();

    private void Start()
    {
        AssignSlot(Managers.Instance.Inventory.PlayerData);
    }

    /// <summary>
    /// �޴������� �ҷ��� ��� �����͸� �ҷ���
    /// </summary>
    /// <param name="playerData">�÷��̾� ������ ������ ��� �����͸� �ҷ���</param>
    public void AssignSlot(PlayerData playerData)
    {
        for (int i = 0; i < playerData.eqiupmentLength; i++)
        {
            equipmentsDictionary.Add(playerData.eqiupments[i], equipments[i]);
            equipments[i].Init(playerData.eqiupments[i]);
        }

        Managers.Instance.Inventory.OnDynamicGearSetChanged += UpdateEquipment;
    }

    private void UpdateEquipment(InventorySlot inventorySlot)
    {
        equipmentsDictionary[inventorySlot].UpdateUISlot(inventorySlot);
    }
}
