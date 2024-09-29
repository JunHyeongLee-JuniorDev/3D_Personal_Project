using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터의 디폴트 데이터 (공격력, 멈추는 거리)
/// </summary>
[CreateAssetMenu(fileName = "MonsterSO", menuName = "Characters/MonsterSO")]
public class MonsterSO : ScriptableObject
{
    [SerializeField] private EMonsterType monsterType;
    [SerializeField] private MonsterSkill[] normalAttacks;
    [SerializeField] private MonsterSkill monsterSkill;


    [Range(0.0f, 5.0f)]
    [SerializeField] private float patrolSpeed;
    [Range(0.0f, 5.0f)]
    [SerializeField] private float runSpeed;
    [Range(0.0f, 30.0f)]
    [SerializeField] private float rotateAroundSpeed;
    [Range(0.0f, 8.0f)]
    [SerializeField] private float rotateDistance;
    [Range(0.0f, 500.0f)]
    [SerializeField] private float maxHealth;

    public EMonsterType MonsterType => monsterType;
    public MonsterSkill[] NormalAttacks => normalAttacks;
    public MonsterSkill MonsterSkill => monsterSkill;
    public float PatrolSpeed => patrolSpeed;
    public float RunSpeed => runSpeed;
    public float RotateAroundSpeed => rotateAroundSpeed;
    public float RotateDistance => rotateDistance;
    public float MaxHealth => maxHealth;    
}



[Serializable]
public class MonsterSkill
{
    [SerializeField]private float damage = 20.0f;
    [SerializeField]private float attackDistance = 1.5f;
    [SerializeField]private float attackAniTime = 1.5f;

    public float Damage => damage;
    public float AttackDistance => attackDistance;
    public float AttackAniTime => attackAniTime;
}