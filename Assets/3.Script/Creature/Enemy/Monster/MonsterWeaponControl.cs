using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeaponControl : MonoBehaviour
{
    [SerializeField] private Collider hitCol;

    public void TurnOnHitCol()
    {
        hitCol.enabled = true;
    }

    public void TurnOffHitCol()
    {
        hitCol.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("�÷��̾� ���� ����");
    }
}
