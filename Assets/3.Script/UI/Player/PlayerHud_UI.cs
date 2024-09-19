using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHud_UI : MonoBehaviour
{
    //���콺 ���÷��̾� Ű�� ���� �߰� �ʿ�-----------------------------------------------------
    //[field : SerializeField] public
    //���콺 ���÷��̾� Ű�� ���� �߰� �ʿ�-----------------------------------------------------

    //Player Sliders
    [field : SerializeField] public FakeSlider_UI playerHp { get; private set; }
    [field : SerializeField] public FakeSlider_UI playerSp { get; private set; }
    [field : SerializeField] public FakeSlider_UI playerAp { get; private set; }

    //Player ESC UI
    [field : SerializeField] public FadeInOut_UI ESCMenu { get; private set; }
    [field: SerializeField] public FadeInOut_UI settings { get; private set; }

    //Player Inventory
    private FadeInOut_UI inventory_UI;
    private FadeInOut_UI gear_UI;
    [field : SerializeField] public DynamicInventoryDisplay gearInv { get; private set; }
    [field : SerializeField] public DynamicInventoryDisplay comsumeInv { get; private set; }
    [field : SerializeField] public DynamicInventoryDisplay missionInv { get; private set; }
    [field: SerializeField] public PlayerSetDisplayer playerCurrentSetDisplay { get; private set; }

    private void Start()
    {
        Managers.Instance.Game.UIInputActions["Cancel"].canceled -= OnClickESC;
        Managers.Instance.Game.UIInputActions["Cancel"].canceled += OnClickESC;

        inventory_UI = gearInv.transform.parent.GetComponent<FadeInOut_UI>();
    }
    public void OnClickESC(InputAction.CallbackContext context)
    {
        if (Managers.Instance.Game.UIGroupStack.Count == 0 &&
            !ESCMenu.gameObject.activeSelf)
        {
            Managers.Instance.Game.playerInput.enabled = false;
            Debug.Log("ESC UI ������ ��");
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
    /// Ÿ��Ʋ ������ �ٽ� �Ѿ�ϴ�. ������ ���� ������ �����մϴ�.
    /// </summary>
    public void OnClickExitGame()
    {
        Managers.Instance.Data.SaveGame(Managers.Instance.Data.currentSaveIndex);
        Managers.Instance.Scene.ChangeScene(EScene.TITLE);
    }
}
