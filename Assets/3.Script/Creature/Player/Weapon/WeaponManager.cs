using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    private Dictionary<string, playerWeaponEvent> weaponModels = new Dictionary<string, playerWeaponEvent>();
    private GameObject axeSkillEff;
    private ParticleSystem axeSkilllparticle;
    private ParticleSystem swordSkillEff;
    private ParticleSystem magicSkillEff;
    private Collider magicSkillCollider;
    private ItemDataBase weaponDB;
    private PlayerData playerData;

    //Normal Attack Collider
    private playerWeaponEvent weaponEvent;

    [SerializeField]private Transform rightHandRoot;
    [SerializeField]private Transform leftHandRoot;

    private PlayerController player;
    private GameObject currentLeftHandModel;
    [SerializeField] private playerWeaponEvent currentRightHandModel;
    private static EWeaponType currentWeaponType = EWeaponType.AXE;
    private playerWeaponEvent bareHand;

    private Coroutine skillcorountineCash;

    /// <summary>
    /// 초기 인벤토리에 있는 무기들의 모델 생성
    /// </summary>
    private void Start()
    {
        axeSkillEff = transform.GetChild(0).gameObject;
        axeSkilllparticle = transform.GetChild(1).GetComponent<ParticleSystem>();
        swordSkillEff = transform.GetChild(2).GetComponent<ParticleSystem>();
        magicSkillEff = transform.GetChild(3).GetComponent<ParticleSystem>();
        magicSkillCollider = magicSkillEff.GetComponent<Collider>();
        player = GetComponentInParent<PlayerController>();

        bareHand = Managers.Instance.InstantiateResouce("Prefabs/Weapons/BareHand", "BareHand").GetComponent<playerWeaponEvent>();
        bareHand.transform.SetParent(rightHandRoot);
        bareHand.gameObject.SetActive(false);

        if (Managers.Instance == null)
        {
            weaponDB = new ItemDataBase();
            weaponDB.LoadDBFromJson();
        }
        else
        {
            weaponDB = Managers.Instance.Data.itemDataBase;
            playerData = Managers.Instance.Inventory.PlayerData;
            playerData.OnWeaponChanged.AddListener(ActivateModel);
        }

            GameObject _newModel = Instantiate(weaponDB.GetModelPrefab("도끼"), rightHandRoot);
            weaponModels.Add("도끼", _newModel.GetComponent<playerWeaponEvent>());
            _newModel.SetActive(false);
            _newModel = Instantiate(weaponDB.GetModelPrefab("검"), rightHandRoot);
            weaponModels.Add("검", _newModel.GetComponent<playerWeaponEvent>());
            _newModel.SetActive(false);
            _newModel = Instantiate(weaponDB.GetModelPrefab("마법장갑"), rightHandRoot);
            weaponModels.Add("마법장갑", _newModel.GetComponent<playerWeaponEvent>());
            _newModel.SetActive(false);
            _newModel = Instantiate(weaponDB.GetModelPrefab("방패"), leftHandRoot);
            weaponModels.Add("방패", _newModel.GetComponent<playerWeaponEvent>());
            _newModel.SetActive(false);

            axeSkillEff.SetActive(false);
            magicSkillCollider.enabled = false;
            ActivateModel();
        }

    /// <summary>
    /// 무기 변환시 불릴 메소드
    /// </summary>
    /// <param name="activeWeaponSlot">활성화되는 무기</param>
    private void ActivateModel()
    {//들고 있는 아이템의 모델 활성화
        if (playerData.equipments[(int)EEquipmentType.Shield].StackSize > 0)
        {
            currentLeftHandModel = weaponModels[playerData.equipments[(int)EEquipmentType.Shield].Data.displayName].gameObject;
            currentLeftHandModel?.SetActive(true);
        }
        else
            currentLeftHandModel?.SetActive(false);

        if (playerData.equipments[(int)EEquipmentType.Weapon].StackSize > 0)
        {
            bareHand.gameObject.SetActive(false);
            currentRightHandModel?.gameObject.SetActive(false);
            currentRightHandModel = weaponModels[playerData.equipments[(int)EEquipmentType.Weapon].Data.displayName];
            currentWeaponType = playerData.equipments[(int)EEquipmentType.Weapon].Data.weaponType;
            currentRightHandModel?.gameObject.SetActive(true);
        }

        else
        {
            Debug.Log("무기 놓기 들어옴");
            currentWeaponType = EWeaponType.None;
            currentRightHandModel?.gameObject.SetActive(false);
            bareHand.gameObject.SetActive(true);
        }
    }

    public void OnNormalCol()
    {
        if (currentWeaponType == EWeaponType.None)
        {
            bareHand.UpdateDamage(20.0f);
            bareHand.Col.enabled = true;
        }

        else
        {
            currentRightHandModel.UpdateDamage(playerData.equipments[(int)EEquipmentType.Weapon].Data.stat);
            currentRightHandModel.Col.enabled = true;
        }
    }

    public void OffNormalCol()
    {
        if (currentWeaponType == EWeaponType.None)
        {
            bareHand.Col.enabled = false;
        }

        else
        {
            currentRightHandModel.Col.enabled = false;
        }
    }


    public void actiaveSkill()
    {
        if (!player.skillTimer.isTickin || currentWeaponType != EWeaponType.None)
        {
            if(skillcorountineCash != null)
                StopCoroutine(skillcorountineCash);

            switch (currentWeaponType)
            {
                case EWeaponType.None:
                    break;

                case EWeaponType.AXE:
                    skillcorountineCash = StartCoroutine(UseAxeSkill());
                    UseAxeSkill();
                    break;
                case EWeaponType.SWORD:
                    UseSwordSkill();
                    break;

                case EWeaponType.MAGIC:
                    skillcorountineCash = StartCoroutine(UseMagicSkill());
                    break;
            }
        }
    }

    private IEnumerator UseAxeSkill()
    {
        axeSkillEff.transform.localPosition = Vector3.zero;

        if (player.m_targetEnemy == null)
        {
            Vector3 _direction = player.transform.forward * 8f;
            
            player.lockOnTarget.position = player.transform.position + _direction;
        }
        axeSkillEff.SetActive(true);
        axeSkilllparticle.Play();

        yield return new WaitForSeconds(1.5f);
        axeSkilllparticle.Stop();

        while (player.skillTimer.isTickin)
        {
            axeSkillEff.transform.position = Vector3.Lerp(axeSkillEff.transform.position, player.lockOnTarget.position, 0.1f);
            yield return null;
        }

        axeSkillEff.SetActive(false);
    }

    private void UseSwordSkill()
    {
        if (playerData.currentMana - playerData.equipments[(int)EWeaponType.SWORD].Data.skillCost <= 0) return;
        swordSkillEff.Play(true);
    }

    private IEnumerator UseMagicSkill()
    {
        yield return new WaitForSeconds(1.0f);
        magicSkillCollider.enabled = true;
        magicSkillEff.Play(true);
    }

    public void endSkill()
    {
        axeSkillEff.SetActive(false);
        magicSkillCollider.enabled = false;
        magicSkillEff.Stop(true);
    }
}
