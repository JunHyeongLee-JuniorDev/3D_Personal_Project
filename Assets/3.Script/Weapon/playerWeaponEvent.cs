using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerWeaponEvent : MonoBehaviour
{
    private PlayerController player;
    private float damage;

    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    public void UpdateDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("���ʹ� �Ǵٴ� ���� ����");
            MonsterController thisMonster = other.GetComponentInParent<MonsterController>();
            thisMonster.ReduceHealth(damage);
            thisMonster.isFoundPlayer = true;
            thisMonster.player = player.transform;
        }
    }
}
