using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerWeaponEvent : MonoBehaviour
{
    public Collider Col { get; private set; }
    private float damage;

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
            Debug.Log("에너미 피다는 로직 실행");
            MonsterController thisMonster = other.GetComponentInParent<MonsterController>();
            thisMonster.ReduceHealth(damage);
            thisMonster.isFoundPlayer = true;
            thisMonster.player = Managers.Instance.Game.playerController.transform;
            thisMonster.stateMachine.OnHurt();
            thisMonster.SprayBlood();
        }
    }
}
