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
    /// 사망, 화톳불 활성화 UI
    /// </summary>
    private TimeOutPopUp_UI diePopUp;
    private TimeOutPopUp_UI bornFirePopUp;

    private Timer resetTimer;

    /*
     * Current Tast
     * 1. 화톳불 UI 만들고 Game Manager에서 켜고 끄기
     * 2. 죽음 UI 만들고 죽음과 동시에 저장하기
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
                Debug.Log("커서 종료");
                CursorLock(true);
            }
        }
    }

    /// <summary>
    /// 플레이어가 죽었을 때, 쉬었을 때
    /// 돈 도 떨궈주구
    /// </summary>
    public void ResetGame()
    {
        Debug.Log("게임 초기화 로직 추가 필요");
        SaveMoneyLocation();
        OnResetGame?.Invoke();
        Debug.Log("리셋 타이머 시간 :" + resetTimer.maxTime);
        resetTimer.StartTimer(() =>
        {
            Debug.Log("타이머 종료");
            Managers.Instance.Scene.ChangeScene(EScene.LOADING);
        });
    }

    /// <summary>
    /// 플레이어 사망시 돈 위치 저장
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

        Debug.Log("죽음 팝업 :" + diePopUp);

        diePopUp.gameObject.SetActive(false);

        bornFirePopUp = Managers.Instance.
                   InstantiateResouce("Prefabs/UI/FirePopUp", "FirePopUp").
                   GetComponent<TimeOutPopUp_UI>();
        Debug.Log("화톳불 팝업 :" + bornFirePopUp);

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
