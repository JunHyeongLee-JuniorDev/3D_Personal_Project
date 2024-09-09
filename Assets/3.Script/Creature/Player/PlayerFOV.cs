using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

[Serializable]
public class PlayerFOV : MonoBehaviour
{
    /*
     * �÷��̾� �þ߰� �������� �ֺ��� ���� �ִٸ� targetting�ؼ� battle State�� �Ѿ�Բ�
     */
    [SerializeField]
    private PlayerController player;

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, player.radiusOfView);
    }
}
