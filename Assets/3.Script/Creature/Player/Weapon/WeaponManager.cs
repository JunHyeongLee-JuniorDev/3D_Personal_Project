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
    private Dictionary<string, GameObject> weaponModels = new Dictionary<string, GameObject>();

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
        bareHand = Managers.Instance.InstantiateResouce("Prefabs/Weapons/BareHand", "BareHand");
        bareHand.SetActive(false);
        GameObject _newModel = Instantiate(Managers.Instance.Data.itemDataBase.GetModelPrefab("도끼"), rightHandRoot);
        weaponModels.Add("도끼", _newModel);
        _newModel.SetActive(false);
        _newModel = Instantiate(Managers.Instance.Data.itemDataBase.GetModelPrefab("검"), rightHandRoot);
        weaponModels.Add("검", _newModel);
        _newModel.SetActive(false);
        _newModel = Instantiate(Managers.Instance.Data.itemDataBase.GetModelPrefab("마법장갑"), rightHandRoot);
        weaponModels.Add("마법장갑", _newModel);
        _newModel.SetActive(false);
        _newModel = Instantiate(Managers.Instance.Data.itemDataBase.GetModelPrefab("방패"), leftHandRoot);
        weaponModels.Add("방패", _newModel);
        _newModel.SetActive(false);

        Managers.Instance.Inventory.PlayerData.OnWeaponChanged.AddListener(ActivateModel);
        ActivateModel();
    }

    /// <summary>
    /// 무기 변환시 불릴 메소드
    /// </summary>
    /// <param name="activeWeaponSlot">활성화되는 무기</param>
    private void ActivateModel()
    {//들고 있는 아이템의 모델 활성화
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
