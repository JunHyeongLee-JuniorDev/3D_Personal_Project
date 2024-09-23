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
    private Dictionary<string, GameObject> weaponModels = new Dictionary<string, GameObject>();

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
        bareHand = Managers.Instance.InstantiateResouce("Prefabs/Weapons/BareHand", "BareHand");
        bareHand.SetActive(false);
        GameObject _newModel = Instantiate(Managers.Instance.Data.itemDataBase.GetModelPrefab("����"), rightHandRoot);
        weaponModels.Add("����", _newModel);
        _newModel.SetActive(false);
        _newModel = Instantiate(Managers.Instance.Data.itemDataBase.GetModelPrefab("��"), rightHandRoot);
        weaponModels.Add("��", _newModel);
        _newModel.SetActive(false);
        _newModel = Instantiate(Managers.Instance.Data.itemDataBase.GetModelPrefab("�����尩"), rightHandRoot);
        weaponModels.Add("�����尩", _newModel);
        _newModel.SetActive(false);
        _newModel = Instantiate(Managers.Instance.Data.itemDataBase.GetModelPrefab("����"), leftHandRoot);
        weaponModels.Add("����", _newModel);
        _newModel.SetActive(false);

        Managers.Instance.Inventory.PlayerData.OnWeaponChanged.AddListener(ActivateModel);
        ActivateModel();
    }

    /// <summary>
    /// ���� ��ȯ�� �Ҹ� �޼ҵ�
    /// </summary>
    /// <param name="activeWeaponSlot">Ȱ��ȭ�Ǵ� ����</param>
    private void ActivateModel()
    {//��� �ִ� �������� �� Ȱ��ȭ
        if (Managers.Instance.Inventory.PlayerData.equipments[(int)EEquipmentType.Shield].StackSize > 0)
        {
            currentLeftHandModel = weaponModels[Managers.Instance.Inventory.PlayerData.equipments[(int)EEquipmentType.Shield].Data.displayName];
            currentLeftHandModel?.SetActive(true);
        }
        else
            currentLeftHandModel?.SetActive(false);

        if (Managers.Instance.Inventory.PlayerData.equipments[(int)EEquipmentType.Weapon].StackSize > 0)
        {
            currentRightHandModel?.SetActive(false);
            currentRightHandModel = weaponModels[Managers.Instance.Inventory.PlayerData.equipments[(int)EEquipmentType.Weapon].Data.displayName];
            currentRightHandModel?.SetActive(true);
        }

        else
        {
            currentLeftHandModel?.SetActive(false);
            bareHand?.SetActive(true);
        }

    }
}
