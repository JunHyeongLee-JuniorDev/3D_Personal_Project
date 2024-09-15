using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowRoot : MonoBehaviour
{
    [SerializeField]
    private Vector3 defualtCamPos;
    [SerializeField]
    private PlayerController player;

    private void Start()
    {
        transform.position = player.transform.position + defualtCamPos;
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position + defualtCamPos;

        if (!player.isBattle)
            transform.forward = player.m_mainCam.transform.forward;
    }
}
