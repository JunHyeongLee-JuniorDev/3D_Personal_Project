using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Events;
using Cinemachine;

public class GameManager : MonoBehaviour, IInitManager
{
    public Stack<FadeInOut_UI> UIGroupStack = new Stack<FadeInOut_UI>();

    //Obj
    public InGamePopUp itemCanvas;
    public PlayerController playerController;
    public PlayerInput playerInput;
    public PlayerHud_UI PlayerHud_UI;
    public InputActionAsset UIInputActions;
    private CinemachineBasicMultiChannelPerlin[] camNoise;

    //Action
    public UnityEvent OnRestGame;
    public UnityAction OnSectionOneEntered;
    public UnityAction OnBossRoomEntered;

    /// <summary>
    /// ���, ȭ��� Ȱ��ȭ UI
    /// </summary>
    private TimeOutPopUp_UI diePopUp;
    private TimeOutPopUp_UI bornFirePopUp;
    private TimeOutPopUp_UI killBossPopUp;

    private Timer resetTimer;
    public Timer restTimer { get; private set; }

    public HashSet<MonsterController> monsters { get; private set; } = new HashSet<MonsterController>();

    /*
     * Current Tast
     * 1. ȭ��� UI ����� Game Manager���� �Ѱ� ����
     * 2. ���� UI ����� ������ ���ÿ� �����ϱ�
     */

    public void Init()
    {
        resetTimer = new Timer(3.0f, this);
        restTimer = new Timer(1.0f, this);
        OnRestGame = new UnityEvent();
        OnRestGame.AddListener(ReviveAllMonsters);
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
    /// ī�޶� �Ҵ� �޼ҵ� GameScene ��ũ��Ʈ���� �Ҵ�
    /// </summary>
    public void AssignCamNoise()
    {
        camNoise = new CinemachineBasicMultiChannelPerlin[playerController.stateDrivenCamera.ChildCameras.Length];

        for (int i = 0; i < playerController.stateDrivenCamera.ChildCameras.Length; i++)
        {
            camNoise[i] = playerController.stateDrivenCamera.ChildCameras[i].GetComponent<CinemachineVirtualCamera>()
                                                                            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }


    }

    public void CamShakeOn()
    {
        Debug.Log("���� ��鸲 ������");
        for (int i = 0; i < camNoise.Length; i++)
        {
            camNoise[i].m_AmplitudeGain = 3.0f;
            camNoise[i].m_FrequencyGain = 1.0f;
        }
    }

    public void CamShakeOff()
    {
        Debug.Log("���� ��鸲 ������");
        for (int i = 0; i < camNoise.Length; i++)
        {
            camNoise[i].m_AmplitudeGain = 0.0f;
            camNoise[i].m_FrequencyGain = 0.0f;
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

        PlayerData _playerData = Managers.Instance.Inventory.PlayerData;
        Managers.Instance.Game.playerInput.enabled = false;

        resetTimer.StartTimer
            (() =>
            {
                _playerData.refillPotion();
                _playerData.currentHealth = _playerData.maxHealth;
                _playerData.currentMana = _playerData.maxMana;
                _playerData.currentStamina = _playerData.maxStamina;

                Managers.Instance.Data.SaveGame(Managers.Instance.Data.currentSaveIndex);
                Debug.Log("���� Ÿ�̸� �ð� :" + resetTimer.maxTime);
                Debug.Log("Ÿ�̸� ����");
                Managers.Instance.Scene.ChangeScene(EScene.LOADING);
            });
    }

    public void RestGame()
    {
        OnRestGame?.Invoke();
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

    public void ReviveAllMonsters()
    {
        foreach (MonsterController monster in monsters)
        {
            monster.gameObject.SetActive(true);
            monster.RefillHealth();
            monster.isDead = false;
            monster.transform.position = monster.statData.defaultSpawnPos;
            monster.OnReset();
        }
    }

    public void CreateDiedPopUp()
    {
        diePopUp = Managers.Instance.
                   InstantiateResouce("Prefabs/UI/DiedPopUp", "DiedPopUp").
                   GetComponent<TimeOutPopUp_UI>();

        diePopUp.gameObject.SetActive(false);

        bornFirePopUp = Managers.Instance.
                   InstantiateResouce("Prefabs/UI/FirePopUp", "FirePopUp").
                   GetComponent<TimeOutPopUp_UI>();

        bornFirePopUp.gameObject.SetActive(false);

        killBossPopUp = Managers.Instance.
                   InstantiateResouce("Prefabs/UI/BossDiePopUp", "BossDiePopUp").
                   GetComponent<TimeOutPopUp_UI>();

        killBossPopUp.gameObject.SetActive(false);  
    }

    public void OnFirePopUp()
    {
        bornFirePopUp.gameObject.SetActive(true);
    }

    public void OnDiedPopUp()
    {
        diePopUp.gameObject.SetActive(true);
    }

    public void OnKillBoss()
    {
        killBossPopUp.gameObject.SetActive(true);
    }
}
