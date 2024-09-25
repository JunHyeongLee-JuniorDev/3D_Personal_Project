using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ����Ʈ ������ (���ݷ�, ���ߴ� �Ÿ�)
/// </summary>
[CreateAssetMenu(fileName = "MonsterSO", menuName = "Characters/MonsterSO")]
public class MonsterSO : ScriptableObject
{
    [SerializeField] private EMonsterType monsterType;
    [SerializeField] private MonsterSkill[] monsterSkills;
    [Range(0.0f, 5.0f)]
    [SerializeField] private float patrolSpeed;
    [Range(0.0f, 5.0f)]
    [SerializeField] private float runSpeed;
    [Range(0.0f, 30.0f)]
    [SerializeField] private float rotateAroundSpeed;

    public EMonsterType MonsterType => monsterType;
    public MonsterSkill[] MonsterSkills => monsterSkills;
    public float PatrolSpeed => patrolSpeed;
    public float RunSpeed => runSpeed;
    public float RotateAroundSpeed => rotateAroundSpeed;
}

[Serializable]
public class MonsterSkill
{
    [SerializeField]private float damage;
    [SerializeField]private float stoppingDistance;
    [SerializeField]private float attackAniTime;

    public float Damage => damage;
    public float StoppingDistance => stoppingDistance;
    public float AttackAniTime => attackAniTime;
}