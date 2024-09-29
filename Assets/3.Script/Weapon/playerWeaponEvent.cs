using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerWeaponEvent : MonoBehaviour
{
    private float damage;

    public void UpdateDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("���ʹ� �Ǵٴ� ���� ����");
            other.GetComponent<MonsterController>().reduceHealth(damage);
        }
    }
}
