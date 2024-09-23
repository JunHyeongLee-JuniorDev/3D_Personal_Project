using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [field: SerializeField] public FadeInOut_UI stats { get; private set; }

    //Player Inventory
    public FadeInOut_UI inventory_UI { get; private set; }

    [field : SerializeField] public DynamicInventoryDisplay gearInv { get; private set; }
    [field : SerializeField] public DynamicInventoryDisplay comsumeInv { get; private set; }
    [field : SerializeField] public DynamicInventoryDisplay missionInv { get; private set; }
    [field: SerializeField] public PlayerSetDisplayer playerCurrentSetDisplay { get; private set; }
    [field: SerializeField] public MouseItemControl mouseItemControl {get; private set;}

    public ItemStatDisplay consumeDisplay {get; private set;}
    public ItemStatDisplay gearDisplay {get; private set;}
    public ItemStatDisplay missionDisplay { get; private set; }

    //Player Status Text
    private TextMeshProUGUI status_Text;

    private void Awake()
    {
        inventory_UI = gearInv.transform.parent.GetComponent<FadeInOut_UI>();
        consumeDisplay = inventory_UI.transform.GetChild(3).GetComponent<ItemStatDisplay>();
        gearDisplay = inventory_UI.transform.GetChild(4).GetComponent<ItemStatDisplay>();
        missionDisplay = inventory_UI.transform.GetChild(5).GetComponent<ItemStatDisplay>();
        status_Text = stats.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;

        Managers.Instance.Game.UIInputActions["Cancel"].started -= OnClickESC;
        Managers.Instance.Game.UIInputActions["Cancel"].started += OnClickESC;

        _playerData.OnRefillStatus.AddListener(OnStatChange);
        _playerData.OnRefillStatus.AddListener(RefillSliders);
        _playerData.OnReduceStatus.AddListener(OnStatChange);
        _playerData.OnReduceStatus.AddListener(ReduceSliders);

        OnStatChange();
        playerHp.ChangeWithoutTween(_playerData.currentHealth, _playerData.maxHealth);
        playerSp.ChangeSlider(_playerData.currentMana, _playerData.maxMana);
        playerAp.ChangeSlider(_playerData.currentStamina, _playerData.maxStamina);
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

    public void OpenStat()
    {
        Managers.Instance.Game.UIGroupStack.Push(stats);
        stats.gameObject.SetActive(true);
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

    public void OnStatChange()
    {
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;

        status_Text.text = $"이름 : {_playerData.name}\n" +
                           $"레벨 : {_playerData.level}\n" +
                           $"체력 : {_playerData.currentHealth} / {_playerData.maxHealth}\t|{_playerData.lifeStat}|\n" +
                           $"활력 : {_playerData.currentStamina} / {_playerData.maxStamina} \t{_playerData.staminaStat}\n" +
                           $"마나 : {_playerData.currentMana} / {_playerData.maxMana}\t|{_playerData.manaStat}|\n" +
                           $"일반 공격력 : {_playerData.weaponDamage} | 스텟 : {_playerData.strengthStat}\n" +
                           $"마법 공력력 : {_playerData.magicDamage} | 스텟 : {_playerData.manaStat}\n";
    }

    public void RefillSliders()
    {
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;
        playerHp.RefillWithFakeSlider(_playerData.currentHealth, _playerData.maxHealth);
        playerSp.ChangeSlider(_playerData.currentMana, _playerData.maxMana);
        playerAp.ChangeSlider(_playerData.currentStamina, _playerData.maxStamina);
    }

    public void ReduceSliders()
    {
        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;
        playerHp.ChangeSliderWithFake(_playerData.currentHealth, _playerData.maxHealth);
        playerSp.ChangeSlider(_playerData.currentMana, _playerData.maxMana);
        playerAp.ChangeSlider(_playerData.currentStamina, _playerData.maxStamina);
    }

    private void OnDestroy()
    {
        Managers.Instance.Game.UIInputActions["Cancel"].started -= OnClickESC;
    }
}
