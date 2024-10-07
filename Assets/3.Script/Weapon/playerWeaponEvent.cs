using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerWeaponEvent : MonoBehaviour
{
    public Collider Col { get; private set; }
    [SerializeField] private float damage;

    private void Start()
    {
        Col = GetComponentInParent<Collider>();
    }

    public void UpdateDamage(float damage)
    {
        Debug.Log("플레이어 데미지 : " + damage);
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log(other.name + "에게 데미지 : " + damage);
            MonsterController thisMonster = other.GetComponentInParent<MonsterController>();
            thisMonster.ReduceHealth(damage);
            if (thisMonster.statData.currentHealth <= 0.0f) other.enabled = false;
            thisMonster.isFoundPlayer = true;
            thisMonster.player = Managers.Instance.Game.playerController.transform;
            thisMonster.stateMachine.OnHurt();
            thisMonster.SprayBlood();
        }
    }
}
