using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerWeaponEvent : MonoBehaviour
{
    public Collider Col { get; private set; }
    [SerializeField] private float damage;
    [SerializeField] private EWeaponType type;

    private void Start()
    {
        Col = GetComponentInParent<Collider>();
    }

    public void UpdateDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Managers.Instance.Sound.Play3DSound("HitSound", transform.position);
            MonsterController thisMonster = other.GetComponentInParent<MonsterController>();
            thisMonster.ReduceHealth(damage);
            if (thisMonster.statData.currentHealth <= 0.0f) other.enabled = false;
            thisMonster.isFoundPlayer = true;
            thisMonster.player = Managers.Instance.Game.playerController.transform;
            thisMonster.stateMachine.OnHurt();
            thisMonster.SprayBlood();

            if (type == EWeaponType.AXE)
                Col.enabled = false;
        }
    }
}
