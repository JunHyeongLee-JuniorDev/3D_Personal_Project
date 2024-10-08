using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class GateControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_cam;
    [SerializeField] private int gateNumber;
    private Animator animator;
    private GameSaveData saveData;
    private BoxCollider col;
    private InGamePopUp popUp;
    private Timer openTimer;
    private bool isGateOpen;

    private void Awake()
    {
        m_cam.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider>();
        openTimer = new Timer(4.0f, this);
        col.enabled = true;
        col.isTrigger = true;
    }

    private void Start()
    {
        saveData = Managers.Instance.Data.currentSaveData[Managers.Instance.Data.currentSaveIndex];
        popUp = Managers.Instance.Game.itemCanvas;

        switch (gateNumber)
        {
            case 0:
                isGateOpen = saveData.isFirstDoorOpened;
                break;

            case 1:
                isGateOpen = saveData.isSecondDoorOpened;
                break;

            case 2:
                isGateOpen = saveData.isBossDoorOpened;
                break;
        }

        if (isGateOpen)
        {
            col.enabled = false;
            animator.Play("Open State");
        }

        else
        {
            col.enabled = true;
            animator.Play("Close State");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && isGateOpen) return;

        popUp.gameObject.SetActive(true);
        popUp.TurnOffIcon();
        popUp.ChangeText("게이트 열기");
        Managers.Instance.Game.playerInput.actions["Interaction"].started -= OpenGate;
        Managers.Instance.Game.playerInput.actions["Interaction"].started += OpenGate;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        popUp.gameObject.SetActive(false);
        Managers.Instance.Game.playerInput.actions["Interaction"].started -= OpenGate;
    }

    private void OpenGate(InputAction.CallbackContext context)
    {
        animator.Play("Main Gate Open");
        Managers.Instance.Game.playerInput.actions["Interaction"].started -= OpenGate;
        popUp.gameObject.SetActive(false);
        m_cam.gameObject.SetActive(true);
        Managers.Instance.Game.playerInput.enabled = false;
        m_cam.m_Priority = 11;
        Managers.Instance.Sound.Play("Environment/Door");
        Managers.Instance.Game.playerController.OffPlayerHitBox();
        Managers.Instance.Game.playerController.CancelAllConditions();
        Managers.Instance.Data.SaveGame(Managers.Instance.Data.currentSaveIndex);

        foreach (MonsterController monster in Managers.Instance.Game.monsters)
        {
            monster.OnPlayerDead();
        }


        openTimer.StartTimer(() =>
        {
            m_cam.m_Priority = -1;
            m_cam.gameObject.SetActive(false);
            col.enabled = false;
            Managers.Instance.Game.playerController.OnPlayerHitBox();
            Managers.Instance.Game.playerInput.enabled = true;
        });

        switch (gateNumber)
        {
            case 0:
                saveData.isFirstDoorOpened = true;
                break;

            case 1:
                saveData.isSecondDoorOpened = true;
                break;

            case 2:
                saveData.isBossDoorOpened = true;
                break;
        }
    }
}
