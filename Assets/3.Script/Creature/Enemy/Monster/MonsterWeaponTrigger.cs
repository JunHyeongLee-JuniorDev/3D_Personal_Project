using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeaponTrigger : MonoBehaviour
{
    private float damage;
    public Rigidbody rb { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void UpdateDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("플레이어 맞음 데미지 : " + damage);

        if (Managers.Instance != null)
        {
            PlayerData _playerData = Managers.Instance.Inventory.PlayerData;

            _playerData.currentHealth -= damage;
            _playerData.OnReduceStatus?.Invoke();
            Managers.Instance.Game.playerController.m_StateMachine.OnHurt();
        }
    }
}
