using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Weapon Item")]
public class WeaponData : InventoryItemData
{
    //장비 모델
    [SerializeField] private GameObject model;
    public GameObject Model => model;

    //장비 종류
    [SerializeField] private EWeaponType weaponType;
    public EWeaponType WeaponType => weaponType;

    //아이템 레벨
    [SerializeField] private int level;
    public int Level => level;

    //아이템 스텟
    [SerializeField] private float stat;
    public float Stat => stat;

    //아이템 강화 상승치
    [SerializeField] private float reinforceStat;
    public float ReinforceStat => reinforceStat;

    //아이템 구매 가격
    [SerializeField] private float buyPrice;
    public float BuyPrice => buyPrice;

    //아이템 강화시 가격 상승치
    [SerializeField] private float reinforceRate;
    public float ReinforceRate => reinforceRate;

    //아이템 강화 가격
    [SerializeField] private float reinforcePrice;
    public float ReinforcePrice => reinforcePrice;

    //아이템 스킬 string
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
    /// 레벨 1, 스텟, 강화 가격 상승
    /// </summary>
    public void Reinforce()
    {
        level += 1;
        stat *= reinforceStat;
        reinforceStat *= reinforceRate;
        reinforceRate *= reinforcePrice;
    }
}
