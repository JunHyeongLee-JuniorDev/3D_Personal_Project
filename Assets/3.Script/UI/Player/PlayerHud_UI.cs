using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHud_UI : MonoBehaviour
{
    //마우스 디스플레이어 키는 로직 추가 필요-----------------------------------------------------
    //[field : SerializeField] public
    //마우스 디스플레이어 키는 로직 추가 필요-----------------------------------------------------

    //Player Sliders
    [field : SerializeField] public FakeSlider_UI playerHp { get; private set; }
    [field : SerializeField] public FakeSlider_UI playerSp { get; private set; }
    [field : SerializeField] public FakeSlider_UI playerAp { get; private set; }

    //Player ESC UI
    [field : SerializeField] public FadeInOut_UI ESCMenu { get; private set; }
    [field: SerializeField] public FadeInOut_UI settings { get; private set; }

    //Player Inventory
    public FadeInOut_UI inventory_UI { get; private set; }
    private FadeInOut_UI gear_UI;
    [field : SerializeField] public DynamicInventoryDisplay gearInv { get; private set; }
    [field : SerializeField] public DynamicInventoryDisplay comsumeInv { get; private set; }
    [field : SerializeField] public DynamicInventoryDisplay missionInv { get; private set; }
    [field: SerializeField] public PlayerSetDisplayer playerCurrentSetDisplay { get; private set; }
    [field: SerializeField] public MouseItemControl mouseItemControl {get; private set;}

    public ItemStatDisplay consumeDisplay {get; private set;}
    public ItemStatDisplay gearDisplay {get; private set;}
    public ItemStatDisplay missionDisplay { get; private set; }

    private void Awake()
    {
        inventory_UI = gearInv.transform.parent.GetComponent<FadeInOut_UI>();
        consumeDisplay = inventory_UI.transform.GetChild(3).GetComponent<ItemStatDisplay>();
        gearDisplay = inventory_UI.transform.GetChild(4).GetComponent<ItemStatDisplay>();
        missionDisplay = inventory_UI.transform.GetChild(5).GetComponent<ItemStatDisplay>();
    }

    private void Start()
    {
        Managers.Instance.Game.UIInputActions["Cancel"].started -= OnClickESC;
        Managers.Instance.Game.UIInputActions["Cancel"].started += OnClickESC;
    }
    public void OnClickESC(InputAction.CallbackContext context)
    {
        if (Managers.Instance.Game.UIGroupStack.Count == 0 &&
            !ESCMenu.gameObject.activeSelf &&
            Managers.Instance.Game.playerController.isGrouded &&
            !Managers.Instance.Game.playerController.isBattle)
        {
            Managers.Instance.Game.playerInput.enabled = false;
            Debug.Log("ESC UI 켜지는 중");
            Managers.Instance.Game.CursorLock(false);
            ESCMenu.gameObject.SetActive(true);
            Managers.Instance.Game.UIGroupStack.Push(ESCMenu);
        }
    }

    public void OpenInv()
    {
        Managers.Instance.Game.UIGroupStack.Push(inventory_UI);
        inventory_UI.gameObject.SetActive(true);
    }

    public void OpenSettings()
    {
        Managers.Instance.Game.UIGroupStack.Push(settings);
        settings.gameObject.SetActive(true);
    }

    /// <summary>
    /// 타이틀 씬으로 다시 넘어갑니다. 가기전 진행 사항을 저장합니다.
    /// </summary>
    public void OnClickExitGame()
    {
        Managers.Instance.Data.SaveGame(Managers.Instance.Data.currentSaveIndex);
        Managers.Instance.Scene.ChangeScene(EScene.TITLE);
    }

    private void OnDestroy()
    {
        Managers.Instance.Game.UIInputActions["Cancel"].started -= OnClickESC;
    }
}
