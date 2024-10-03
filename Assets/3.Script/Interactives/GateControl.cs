using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class GateControl : MonoBehaviour
{
    [SerializeField] private int gateNumber;
    private Animator animator;
    private GameSaveData saveData;
    private BoxCollider col;
    private InGamePopUp popUp;
    private bool isGateOpen;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider>();
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
            animator.Play("Open State");
        }

        else
        {
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
        popUp.gameObject.SetActive(false);

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
