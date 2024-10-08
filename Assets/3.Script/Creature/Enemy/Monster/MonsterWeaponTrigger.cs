using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeaponTrigger : MonoBehaviour
{
    [SerializeField] private float damage;
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
        if (!other.CompareTag("Player") || Managers.Instance == null) return;

        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;
        Managers.Instance.Sound.Play3DSound("HitSound", transform.position);

        _playerData.currentHealth -= damage;
        if (_playerData.currentHealth <= 0) other.enabled = false;

        _playerData.OnReduceStatus?.Invoke();
        Managers.Instance.Game.playerController.m_StateMachine.OnHurt();
        ParticleSystem _blood = other.GetComponentInChildren<ParticleSystem>();

        Vector3 bloodTarget = transform.position;
        bloodTarget.y = _blood.transform.position.y;

        _blood.transform.LookAt(bloodTarget);
        _blood.Play();
    }
}
