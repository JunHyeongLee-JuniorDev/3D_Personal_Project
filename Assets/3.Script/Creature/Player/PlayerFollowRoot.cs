using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowRoot : MonoBehaviour
{
    [SerializeField]
    private Vector3 defualtCamPos;
    [SerializeField]
    private Transform player;

    private void Start()
    {
        transform.position = player.position + defualtCamPos;
    }

    private void LateUpdate()
    {
        transform.position = player.position + defualtCamPos;
    }
}
