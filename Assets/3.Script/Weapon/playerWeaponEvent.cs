using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerWeaponEvent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("에너미 피다는 로직 실행");
        }
    }
}
