using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameManager : MonoBehaviour, IInitManager
{
    public Stack<FadeInOut_UI> UIGroupStack = new Stack<FadeInOut_UI>();

    //Obj
    public InGamePopUp itemCanvas;
    public PlayerController playerController;
    public PlayerInput playerInput;
    public PlayerHud_UI PlayerHud_UI;
    public InputActionAsset UIInputActions;

    //Action
    public UnityAction OnResetGame;
    public UnityAction OnSectionOneEntered;
    public UnityAction OnBossRoomEntered;

    /// <summary>
    /// ���, ȭ��� Ȱ��ȭ UI
    /// </summary>
    private TimeOutPopUp_UI diePopUp;
    private TimeOutPopUp_UI bornFirePopUp;

    private Timer resetTimer;

    /*
     * Current Tast
     * 1. ȭ��� UI ����� Game Manager���� �Ѱ� ����
     * 2. ���� UI ����� ������ ���ÿ� �����ϱ�
     */

    public void Init()
    {
        resetTimer = new Timer(3.0f, this);
    }

    public void OnClickExit_UIGroup(InputAction.CallbackContext context)
    {
        if (UIGroupStack.Count > 0)
        {
            UIGroupStack.Pop()?.OnClickFade_Btn();

            if (playerInput != null &&
                UIGroupStack.Count == 0)
            {
                Managers.Instance.Game.playerInput.enabled = true;
                Debug.Log("Ŀ�� ����");
                CursorLock(true);
            }
        }
    }

    /// <summary>
    /// �÷��̾ �׾��� ��, ������ ��
    /// �� �� �����ֱ�
    /// </summary>
    public void ResetGame()
    {
        Debug.Log("���� �ʱ�ȭ ���� �߰� �ʿ�");
        SaveMoneyLocation();
        OnResetGame?.Invoke();
        Debug.Log("���� Ÿ�̸� �ð� :" + resetTimer.maxTime);
        resetTimer.StartTimer(() =>
        {
            Debug.Log("Ÿ�̸� ����");
            Managers.Instance.Scene.ChangeScene(EScene.LOADING);
        });
    }

    /// <summary>
    /// �÷��̾� ����� �� ��ġ ����
    /// </summary>
    private void SaveMoneyLocation()
    {
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;

        _playerData.droppedMoney = _playerData.wallet;
        _playerData.wallet = 0;
        _playerData.droppedMoneyPos = playerController.transform.position;
    }

    public void CursorLock(bool isLock)
    {
        if (isLock)
          Cursor.lockState = CursorLockMode.Locked;
        
        else
            Cursor.lockState = CursorLockMode.None;
        
        
            Cursor.visible = !isLock;
    }

    public void CreateDiedPopUp()
    {
        diePopUp = Managers.Instance.
                   InstantiateResouce("Prefabs/UI/DiedPopUp", "DiedPopUp").
                   GetComponent<TimeOutPopUp_UI>();

        Debug.Log("���� �˾� :" + diePopUp);

        diePopUp.gameObject.SetActive(false);

        bornFirePopUp = Managers.Instance.
                   InstantiateResouce("Prefabs/UI/FirePopUp", "FirePopUp").
                   GetComponent<TimeOutPopUp_UI>();
        Debug.Log("ȭ��� �˾� :" + bornFirePopUp);

        bornFirePopUp.gameObject.SetActive(false);
    }

    public void OnFirePopUp()
    {
        bornFirePopUp.gameObject.SetActive(true);
    }

    public void OnDiedPopUp()
    {
        diePopUp.gameObject.SetActive(true);
    }
}
