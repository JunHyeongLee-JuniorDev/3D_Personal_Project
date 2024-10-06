using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    /// �ʱ� �κ��丮�� �ִ� ������� �� ����
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

            GameObject _newModel = Instantiate(weaponDB.GetModelPrefab("����"), rightHandRoot);
            weaponModels.Add("����", _newModel.GetComponent<playerWeaponEvent>());
            _newModel.SetActive(false);
            _newModel = Instantiate(weaponDB.GetModelPrefab("��"), rightHandRoot);
            weaponModels.Add("��", _newModel.GetComponent<playerWeaponEvent>());
            _newModel.SetActive(false);
            _newModel = Instantiate(weaponDB.GetModelPrefab("�����尩"), rightHandRoot);
            weaponModels.Add("�����尩", _newModel.GetComponent<playerWeaponEvent>());
            _newModel.SetActive(false);
            _newModel = Instantiate(weaponDB.GetModelPrefab("����"), leftHandRoot);
            weaponModels.Add("����", _newModel.GetComponent<playerWeaponEvent>());
            _newModel.SetActive(false);

            axeSkillEff.SetActive(false);
            magicSkillCollider.enabled = false;
            ActivateModel();
        }

    /// <summary>
    /// ���� ��ȯ�� �Ҹ� �޼ҵ�
    /// </summary>
    /// <param name="activeWeaponSlot">Ȱ��ȭ�Ǵ� ����</param>
    private void ActivateModel()
    {//��� �ִ� �������� �� Ȱ��ȭ
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
            Debug.Log("���� ���� ����");
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
