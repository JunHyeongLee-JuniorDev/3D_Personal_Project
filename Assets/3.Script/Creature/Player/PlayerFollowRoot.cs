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
        player.OnPlayerDead -= targetRootOff;
        player.OnPlayerDead += targetRootOff;
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position + defualtCamPos;

        if (!player.isBattle)
            transform.forward = player.m_mainCam.transform.forward;
    }

    public void AssignPlayer(PlayerController player)
    {
        this.player = player;
    }

    private void targetRootOff()
    {
        gameObject.SetActive(false);
    }
}
