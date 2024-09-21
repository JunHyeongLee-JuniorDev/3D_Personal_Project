using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    /*
     * �ִϸ��̼��� weapon�� ������ ����
     * �ٲٴ� �ִϸ��̼��� �κ��丮���� ����
     * �ٲٰ� ���� �� �ٲٱ� ����
     * weapon�� ��ġ�� transform�� ���� �־���Ѵ�.
     * �޼� �׸��� ������
     * ���� �ٲܶ� �̺�Ʈ ������ֱ�
     * �κ��丮�� �ִ� ��� ��� �������� �𵨵� ������ ����
     * ������ �ݶ��̴� ��ũ��Ʈ �����ϱ�
     */
    private Dictionary<WeaponData, GameObject> weaponModels = new Dictionary<WeaponData, GameObject>();

    [SerializeField]private Transform rightHandRoot;
    [SerializeField]private Transform leftHandRoot;
    private GameObject currentLeftHandModel;
    private GameObject currentRightHandModel;
    private GameObject bareHand;

    /// <summary>
    /// �ʱ� �κ��丮�� �ִ� ������� �� ����
    /// </summary>
    private void Start()
    {
        foreach (InventorySlot slot in Managers.Instance.Inventory.InvSys_Gear.InventorySlots)
        {// �κ��丮 ���� ��ȸ
            if (slot.ItemData != null)
            {// �κ��丮�� �����ϸ�
                WeaponData _weaponData = slot.ItemData as WeaponData;// ����� �ɽ���

                if (_weaponData.Model != null)
                {// ���� �����Ѵٸ�
                    if (!_weaponData.WeaponType.Equals(EWeaponType.SHIELD))
                    {// �Ϲ� ������ �����տ� ����
                        CreateNewWeapon(_weaponData, rightHandRoot);
                    }

                    else
                    {// ���ж�� �޼տ� ����
                        CreateNewWeapon(_weaponData, leftHandRoot);
                    }
                }

                else//�� �Ҵ� ���
                    Debug.Log("���� �Ҵ���� ����");
            }
        }
        bareHand = Managers.Instance.InstantiateResouce("Prefabs/Weapons/BareHand", "BareHand");
        Managers.Instance.Inventory.PlayerData.OnWeaponChanged += ActivateModel;
        Managers.Instance.Inventory.InvSys_Gear.OnInventorySlotChanged += UpdateModel;

        if(Managers.Instance.Inventory.PlayerData.equipments[(int)EEquipmentType.Shield].ItemData != null)
            ActivateModel(Managers.Instance.Inventory.PlayerData.equipments[(int)EEquipmentType.Shield]);

        if (Managers.Instance.Inventory.PlayerData.equipments[(int )EEquipmentType.Weapon].ItemData != null)
            ActivateModel(Managers.Instance.Inventory.PlayerData.equipments[(int)EEquipmentType.Weapon]);
    }

    /// <summary>
    /// ���� ����ϴ� ���̶�� ����/���� ���̶�� ����
    /// </summary>
    /// <param name="slot"></param>
    private void UpdateModel(InventorySlot slot)
    {
        WeaponData _weaponData = slot.ItemData as WeaponData;

        if (_weaponData == null)
        {// �������� ������Ŷ��
            // ������ �������� ���� key�� �Ҿ������.

            foreach (var _modelKey in weaponModels.Keys)
            {//Ű�� �� ã�Ƽ� ������ dictionary���� ���� X �ش� ���� �κ��� ���ٸ� dictionary���� ����
                bool isInInventory = false;

                foreach (var _invSlot in Managers.Instance.Inventory.InvSys_Gear.InventorySlots)
                {
                    if (weaponModels[_modelKey].Equals((_invSlot.ItemData as WeaponData).Model))
                    {//���� key�� �ش��ϴ� �𵨰� ���� ���� ���� �� true
                        isInInventory = true;
                    }
                }

                if (!isInInventory)
                {//���� ���� �κ��� �������� �ʴٸ� ����
                    Destroy(weaponModels[_modelKey]);
                    weaponModels.Remove(_modelKey);
                }
            }
        }

        else if (!weaponModels.ContainsKey(_weaponData))
        {//�߰��� ������ ��ųʸ��� ���ٸ� ���ο� ���� �����ϰ� ��ųʸ��� ���
            if (_weaponData.WeaponType.Equals((int)EWeaponType.SHIELD))
                CreateNewWeapon(_weaponData, leftHandRoot);

            else
                CreateNewWeapon(_weaponData, rightHandRoot);
        }
    }
    /// <summary>
    /// ���� ��ȯ�� �Ҹ� �޼ҵ�
    /// </summary>
    /// <param name="activeWeaponSlot">Ȱ��ȭ�Ǵ� ����</param>
    private void ActivateModel(InventorySlot activeWeaponSlot)
    {//��� �ִ� �������� �� Ȱ��ȭ
        WeaponData weaponData = activeWeaponSlot.ItemData as WeaponData;

        if (weaponData != null)
        {
            if(weaponData.WeaponType.Equals(EWeaponType.SHIELD))
            {
                currentLeftHandModel?.SetActive(false);
                currentLeftHandModel = weaponModels[weaponData];
                currentLeftHandModel.SetActive(true);
            }

            else
            {
                currentRightHandModel?.SetActive(false);
                currentRightHandModel = weaponModels[weaponData];
                currentRightHandModel.SetActive(true);
            }
        }

        else if(Managers.Instance.Inventory.PlayerData.equipments[(int)EEquipmentType.Shield].ItemData == null)
        {
            currentLeftHandModel?.SetActive(false);
        }

        else if (Managers.Instance.Inventory.PlayerData.equipments[(int)EEquipmentType.Weapon].ItemData == null)
        {
            currentRightHandModel?.SetActive(false);
            currentRightHandModel = bareHand;
            currentRightHandModel.SetActive(true);
        }

    }

    private void CreateNewWeapon(WeaponData newWeaponData, Transform parent)
    {
        GameObject newWeapon = Instantiate(newWeaponData.Model, parent);
        weaponModels.Add(newWeaponData, newWeapon);
        newWeapon.SetActive(false);
    }
}
