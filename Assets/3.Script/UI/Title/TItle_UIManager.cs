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

    private int selectedFileIndex;

    private void Start()
    {
        popup_Text = popUpPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
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
                Debug.Log("false는 없는 것이냐...");
                newGameSlots[i].SetActive(true);
                existGameSlots[i].SetActive(false);
            }

            else
            {
                newGameSlots[i].SetActive(false);
                existGameSlots[i].SetActive(true);

                existGameSlots[i].GetComponentInChildren<TextMeshProUGUI>().text =
                    $"레벨 : {_saveDatas[i].savePlayerData.level}\n" +
                    $"돈 : {_saveDatas[i].savePlayerData.wallet}";
            }
        }
    }

    public void OnClickDelete(int fileIndex)
    {
        selectedFileIndex = fileIndex;
        popUpPanel.SetActive(true);
        Managers.Instance.Game.UIGroups?.Push(popUpPanel);

        popup_Text.text = "정말 삭제하시겠습니까?";
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

        popup_Text.text = "정말 새 게임을 생성하시겠습니까?";
        popupYes_Btn.onClick.RemoveAllListeners();
        popupYes_Btn.onClick.AddListener(CreateNewGame);
        popupYes_Btn.onClick.AddListener(() =>
        {
            Managers.Instance.Game.UIGroups.Pop().SetActive(false);
        });
    }

    public void OnClickNextScene()
    {
        Managers.Instance.Scene.ChangeScene(EScene.PLAYER);// for debug
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
        Managers.Instance.Data.currentSaveData[selectedFileIndex] = new GameSaveData();
        Managers.Instance.Data.SaveGame(selectedFileIndex);
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
