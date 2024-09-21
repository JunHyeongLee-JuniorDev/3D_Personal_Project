using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    /*
     * 애니메이션은 weapon이 가지고 있음
     * 바꾸는 애니메이션은 인벤토리에서 진행
     * 바꾸고 있을 때 바꾸기 가능
     * weapon을 위치할 transform을 갖고 있어야한다.
     * 왼손 그리고 오른손
     * 무기 바꿀때 이벤트 등록해주기
     * 인벤토리에 있는 모든 장비 아이템의 모델들 생성해 놓기
     * 무기의 콜라이더 스크립트 참조하기
     */
    private Dictionary<WeaponData, GameObject> weaponModels = new Dictionary<WeaponData, GameObject>();

    [SerializeField]private Transform rightHandRoot;
    [SerializeField]private Transform leftHandRoot;
    private GameObject currentLeftHandModel;
    private GameObject currentRightHandModel;
    private GameObject bareHand;

    /// <summary>
    /// 초기 인벤토리에 있는 무기들의 모델 생성
    /// </summary>
    private void Start()
    {
        foreach (InventorySlot slot in Managers.Instance.Inventory.InvSys_Gear.InventorySlots)
        {// 인벤토리 슬롯 순회
            if (slot.ItemData != null)
            {// 인벤토리가 존재하면
                WeaponData _weaponData = slot.ItemData as WeaponData;// 무기로 케스팅

                if (_weaponData.Model != null)
                {// 모델이 존재한다면
                    if (!_weaponData.WeaponType.Equals(EWeaponType.SHIELD))
                    {// 일반 무기라면 오른손에 생성
                        CreateNewWeapon(_weaponData, rightHandRoot);
                    }

                    else
                    {// 방패라면 왼손에 생성
                        CreateNewWeapon(_weaponData, leftHandRoot);
                    }
                }

                else//모델 할당 요망
                    Debug.Log("모델이 할당되지 않음");
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
    /// 새로 등록하는 것이라면 생성/비우는 것이라면 제거
    /// </summary>
    /// <param name="slot"></param>
    private void UpdateModel(InventorySlot slot)
    {
        WeaponData _weaponData = slot.ItemData as WeaponData;

        if (_weaponData == null)
        {// 아이템이 비워진거라면
            // 문제점 아이템을 비우면 key를 잃어버린다.

            foreach (var _modelKey in weaponModels.Keys)
            {//키로 모델 찾아서 있으면 dictionary에서 제거 X 해당 모델이 인벤에 없다면 dictionary에서 제거
                bool isInInventory = false;

                foreach (var _invSlot in Managers.Instance.Inventory.InvSys_Gear.InventorySlots)
                {
                    if (weaponModels[_modelKey].Equals((_invSlot.ItemData as WeaponData).Model))
                    {//만약 key에 해당하는 모델과 같은 모델이 있을 시 true
                        isInInventory = true;
                    }
                }

                if (!isInInventory)
                {//만약 모델이 인벤에 존재하지 않다면 제거
                    Destroy(weaponModels[_modelKey]);
                    weaponModels.Remove(_modelKey);
                }
            }
        }

        else if (!weaponModels.ContainsKey(_weaponData))
        {//추가된 슬롯이 딕셔너리에 없다면 새로운 모델을 생성하고 딕셔너리에 등록
            if (_weaponData.WeaponType.Equals((int)EWeaponType.SHIELD))
                CreateNewWeapon(_weaponData, leftHandRoot);

            else
                CreateNewWeapon(_weaponData, rightHandRoot);
        }
    }
    /// <summary>
    /// 무기 변환시 불릴 메소드
    /// </summary>
    /// <param name="activeWeaponSlot">활성화되는 무기</param>
    private void ActivateModel(InventorySlot activeWeaponSlot)
    {//들고 있는 아이템의 모델 활성화
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
