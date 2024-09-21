using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Weapon Item")]
public class WeaponData : InventoryItemData
{
    //��� ��
    [SerializeField] private GameObject model;
    public GameObject Model => model;

    //��� ����
    [SerializeField] private EWeaponType weaponType;
    public EWeaponType WeaponType => weaponType;

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

    //������ ��ų string
    [SerializeField] private string skillstring;

    public string Skillstring => skillstring;

    [SerializeField] private float comboBtnCoolTime;
    public float ComboBtnCoolTime => comboBtnCoolTime;

    [TextArea(1, 4)]
    [SerializeField] private string[] animationClips;
    public string[] AnimationClips
    {
        get { return animationClips; }
    }

    [TextArea(1, 4)]
    [SerializeField] private string skillAniClip;

    public string SkillAniClip
    {
        get { return skillAniClip; }
    }

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
