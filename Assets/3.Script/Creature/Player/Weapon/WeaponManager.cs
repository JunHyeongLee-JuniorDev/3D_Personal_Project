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
    private Dictionary<string, GameObject> weaponModels = new Dictionary<string, GameObject>();
    private GameObject axeSkillEff;
    private ParticleSystem axeSkilllparticle;
    private ParticleSystem swordSkillEff;
    private ParticleSystem magicSkillEff;
    private Collider magicSkillCollider;

    [SerializeField]private Transform rightHandRoot;
    [SerializeField]private Transform leftHandRoot;

    private PlayerController player;
    private GameObject currentLeftHandModel;
    private GameObject currentRightHandModel;
    private static EWeaponType currentWeaponType = EWeaponType.AXE;
    private GameObject bareHand;

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
            currentWeaponType = Managers.Instance.Inventory.PlayerData.equipments[(int)EEquipmentType.Weapon].Data.weaponType;
            currentRightHandModel?.SetActive(true);
        }

        else
        {
            currentWeaponType = EWeaponType.None;
            currentLeftHandModel?.SetActive(false);
            bareHand?.SetActive(true);
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
        swordSkillEff.Stop(true);
        magicSkillEff.Stop(true);
    }
}
