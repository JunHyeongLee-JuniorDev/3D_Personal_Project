using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Title_UIManager : MonoBehaviour
{
    //Each UI Obj
    [SerializeField] private GameObject loadGame_UI;
    [SerializeField] private GameObject settings_UI;
    [SerializeField] private GameObject more_UI;

    //frist selected button
    [SerializeField] private Button titlePlay_Btn;
    [SerializeField] private Button settingFirst_Btn;
    [SerializeField] private Button moreFirst_Btn;

    //objs
    [SerializeField] private GameObject[] newGameSlots;
    [SerializeField] private GameObject[] existGameSlots;
    [SerializeField] private GameObject popUpPanel;

    //popup UI
    private Button popupYes_Btn;
    private Button popupNo_Btn;
    private TextMeshProUGUI popup_Text;
    private TextMeshProUGUI popupWarnning_Text;
    private TMP_InputField popupInput_InputField;

    //cashing
    private int selectedFileIndex;
    private string createdUserName = string.Empty;

    private void Start()
    {
        popup_Text = popUpPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        popupInput_InputField = popUpPanel.transform.GetChild(3).GetChild(0).GetComponent<TMP_InputField>();
        popupWarnning_Text = popUpPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        popupYes_Btn = popUpPanel.transform.GetChild(1).GetComponent<Button>();
        popupNo_Btn = popUpPanel.transform.GetChild(2).GetComponent<Button>();

        popupNo_Btn.onClick.AddListener(() => 
        { 
            Managers.Instance.Game.UIGroups.Pop().SetActive(false); 
        });
    }

    public void OnClickPlay()
    {
        Managers.Instance.Game.UIGroups?.Push(loadGame_UI);

        RefreshPlayPanel();
        loadGame_UI.SetActive(true);
    }

    public void RefreshPlayPanel()
    {
        for (int i = 0; i < Managers.Instance.Data.currentSaveData.Length; i++)
        {
            Managers.Instance.Data.LoadGame(i);
        }

        GameSaveData[] _saveDatas = Managers.Instance.Data.currentSaveData;

        for (int i = 0; i < _saveDatas.Length; i++)
        {
            if (_saveDatas[i] == null)
            {
                newGameSlots[i].SetActive(true);
                existGameSlots[i].SetActive(false);
            }

            else
            {
                newGameSlots[i].SetActive(false);
                existGameSlots[i].SetActive(true);

                existGameSlots[i].GetComponentInChildren<TextMeshProUGUI>().text =
                    $"�̸� : {_saveDatas[i].savePlayerData.name}\n" +
                    $"���� : {_saveDatas[i].savePlayerData.level}\n" +
                    $"�� : {_saveDatas[i].savePlayerData.wallet}";
            }
        }
    }

    /// <summary>
    /// delete ��ư�� popup�޴��� 
    /// </summary>
    /// <param name="fileIndex"></param>
    public void OnClickDelete(int fileIndex)
    {
        selectedFileIndex = fileIndex;
        popUpPanel.SetActive(true);
        Managers.Instance.Game.UIGroups?.Push(popUpPanel);

        popup_Text.text = "���� �����Ͻðڽ��ϱ�?";
        popupWarnning_Text.text = string.Empty;
        createdUserName = string.Empty;
        popupInput_InputField.transform.parent.gameObject.SetActive(false);
        popupYes_Btn.onClick.RemoveAllListeners();
        popupYes_Btn.onClick.AddListener(DeleteSaveFile);
        popupYes_Btn.onClick.AddListener(() =>
        {
            Managers.Instance.Game.UIGroups.Pop().SetActive(false);
        });
    }

    public void OnClickCreate(int fileIndex)
    {
        selectedFileIndex = fileIndex;
        popUpPanel.SetActive(true);
        Managers.Instance.Game.UIGroups?.Push(popUpPanel);

        popup_Text.text = "���� �� ������ �����Ͻðڽ��ϱ�?";
        popupWarnning_Text.text = string.Empty;
        createdUserName = string.Empty;
        popupInput_InputField.transform.parent.gameObject.SetActive(true);
        popupInput_InputField.text = string.Empty;
        popupYes_Btn.onClick.RemoveAllListeners();
        popupYes_Btn.onClick.AddListener(CreateNewGame);
    }

    /// <summary>
    /// ������ �������� �� �����͸� �ҷ����鼭 �� ��ȯ�ϱ�
    /// </summary>
    /// <param name="fileIndex"> ������ �ε����� �ް������� �޾� ������ �޴����� ���� </param>
    public void OnClickNextScene(int fileIndex)
    {
        Managers.Instance.Data.currentSaveIndex = fileIndex;
        Managers.Instance.Scene.ChangeScene(EScene.GAME);// for debug
    }

    public void OnEnterUserName()
    {
        createdUserName = popupInput_InputField.text;
    }

    public void DeleteSaveFile()
    {
        Managers.Instance.Data.DeleteGameSave(selectedFileIndex);
        newGameSlots[selectedFileIndex].SetActive(true);
        existGameSlots[selectedFileIndex].SetActive(false);
        RefreshPlayPanel();
    }

    public void CreateNewGame()
    {
        if (createdUserName.Equals(string.Empty))
        {
            popupWarnning_Text.text = "�̸��� �Է����ּ���.";
            return;
        }

        Managers.Instance.Game.UIGroups.Pop().SetActive(false);
        Managers.Instance.Data.CreateNewGame(selectedFileIndex, createdUserName);
        RefreshPlayPanel();
    }

    public void OnClickSettings()
    {
        Managers.Instance.Game.UIGroups?.Push(settings_UI);
        settings_UI.SetActive(true);
        settingFirst_Btn.Select();
    }

    public void OnClickMore()
    {
        Managers.Instance.Game.UIGroups?.Push(more_UI);
        more_UI.SetActive(true);
        moreFirst_Btn.Select();
    }

    public void ClosePopUp()
    {
        if (Managers.Instance.Game.UIGroups.Count > 0)
            Managers.Instance.Game.UIGroups?.Pop().SetActive(false);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickGitURL()
    {
        Application.OpenURL("https://github.com/JunHyeongLee-JuniorDev/3D_Personal_Project");
    }
}
