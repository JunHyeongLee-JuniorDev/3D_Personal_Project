using UnityEngine;
using System;
using System.IO;
using UnityEngine.Audio;

public class DataManager : MonoBehaviour, IInitManager
{
    //Save Files
    public GameSaveData[] currentSaveData;
    public SettingSaveData settingSaveData;
    public AudioMixer audioMixer;

    //���� ���̺� ���� index
    public int currentSaveIndex;

    // ���� ���̺� ������ �����
    private SettingSaveData defaultSetting = new SettingSaveData();
    public SettingSaveData DefaultSetting => defaultSetting;

    //file name hardcoding
    public readonly string saveDirectory = "/SaveData/";
    public readonly string[] gameSaveNames = new string[] {"SaveGame0.sav", 
                                                           "SaveGame1.sav",
                                                           "SaveGame2.sav"};
    public readonly string settingfileName = "SettingSave.sav";

    public readonly string itemModelPath = "Prefabs/ItemObj";
    public readonly string[] equipmentModelPaths = new string[] { ""};

    public void Init()
    {
        defaultSetting.masterVolume = 0.0f;
        defaultSetting.SFXVolume = 0.0f;
        defaultSetting.musicVolume = 0.0f;
        defaultSetting.resolutionIndex = Screen.resolutions.Length - 1;
        defaultSetting.urpAssetsIndex = QualitySettings.names.Length - 1;
        defaultSetting.isFullScreen = true;

        currentSaveData = new GameSaveData[gameSaveNames.Length];

        for (int i = 0; i < currentSaveData.Length; i++)
        {
            LoadGame(i);
        }

        settingSaveData = LoadSetting();

        QualitySettings.SetQualityLevel(settingSaveData.urpAssetsIndex, true);
        Screen.SetResolution(Screen.resolutions[settingSaveData.resolutionIndex].width,
                             Screen.resolutions[settingSaveData.resolutionIndex].height,
                             settingSaveData.isFullScreen);

        audioMixer = audioMixer = Resources.Load<AudioMixer>("Mixer/AudioMixer");
    }

    private void Start()
    {
        audioMixer.SetFloat("Master", settingSaveData.masterVolume);
        audioMixer.SetFloat("Music", settingSaveData.musicVolume);
        audioMixer.SetFloat("SFX", settingSaveData.SFXVolume);
    }

    /// <summary>
    /// ������ ���� �ε����� ����
    /// </summary>
    /// <param name="saveFileIndex"></param>
    /// <returns>���� ������ true�� ��ȯ</returns>
    public bool SaveGame(int saveFileIndex)
    {
        string _dir = Application.persistentDataPath + saveDirectory;

        if (!Directory.Exists(_dir))
            Directory.CreateDirectory(_dir);

        string _json = JsonUtility.ToJson(currentSaveData[saveFileIndex], prettyPrint : true);
        /*
         * 
         * prettyprint = true  -> �б� ����
         * prettyprint = false -> ���� �뷮 ����ȭ
         */

        File.WriteAllText(path: _dir + gameSaveNames[saveFileIndex], contents: _json);

        GUIUtility.systemCopyBuffer = _dir;
        Debug.Log($"Save ��� : {_dir}");
        // ���� ��θ� üũ�ϱ� ���ؼ�

        //�� �ε� ���� ��� ��� save�� �̻��� �� ������ �ִٸ� üũ�Ͽ� false�� ��ȯ����.
        
        return true;
    }

    public void CreateNewGame(int saveFileIndex, string userName)
    {
        GameSaveData _newGamedata = new GameSaveData();
        _newGamedata.savePlayerData.name = userName;

        currentSaveData[saveFileIndex] = _newGamedata;
        SaveGame(saveFileIndex);
    }

    /// <summary>
    /// ���� ���̺� ������ �ε����� �޾Ƽ� �θ�
    /// </summary>
    /// <param name="fileIndex">������ �޴����� ���� �̸� string index�� ������ ã��</param>
    /// <returns>���丮�� �ִ� ������ return ���� �� default ���� return</returns>
    public void LoadGame(int fileIndex)
    {
        string fullPath = Application.persistentDataPath + saveDirectory + gameSaveNames[fileIndex];
        GameSaveData loadedData = new GameSaveData();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            loadedData = JsonUtility.FromJson<GameSaveData>(json);
            currentSaveData[fileIndex] = loadedData;
        }

        else
        {
            Debug.Log(message: "Save Data does not exist!");
            currentSaveData[fileIndex] = null;
        }

    }
    /// <summary>
    /// ���� ������ ���丮���� �θ�
    /// </summary>
    /// <returns>���丮�� �ִ� ������ return ���� �� default ������ return</returns>
    public SettingSaveData LoadSetting()
    {
        string fullPath = Application.persistentDataPath + saveDirectory + settingfileName;
        SettingSaveData loadedData = defaultSetting;

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            loadedData = JsonUtility.FromJson<SettingSaveData>(json);
        }

        else
        {
            Debug.Log(message: "Save Data does not exist!");
        }

        return loadedData;
    }
    public bool SaveSetting()
    {
        string _dir = Application.persistentDataPath + saveDirectory;

        if (!Directory.Exists (_dir))
            Directory.CreateDirectory(_dir);

        string json = JsonUtility.ToJson(settingSaveData, prettyPrint : true);

        File.WriteAllText(path : _dir + settingfileName, contents : json);
        GUIUtility.systemCopyBuffer = _dir;
        Debug.Log($"���� ���� ��� {_dir}");

        return true;
    }

    public void DeleteGameSave(int fileIndex)
    {
        string filePath = Application.persistentDataPath + saveDirectory + gameSaveNames[fileIndex];

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        else
        {
            Debug.Log("�ش� ������ �����ϴ�.");
        }
    }
}