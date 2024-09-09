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
     * 플레이어 시야각 구현하자 주변에 적이 있다면 targetting해서 battle State로 넘어가게끔
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
