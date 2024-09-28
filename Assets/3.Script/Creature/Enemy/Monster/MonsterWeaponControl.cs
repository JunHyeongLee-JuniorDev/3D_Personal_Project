using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeaponControl : MonoBehaviour
{
    [SerializeField]private Collider hitCol;

    public void TurnOnHitCol()
    {
        hitCol.enabled = true;
    }

    public void TurnOffHitCol()
    {
        hitCol.enabled = false;
    }
}
