using UnityEngine;
using System;
using System.IO;

public class DataManager : MonoBehaviour//, IInitManager
{
    public static DataManager instance = null;
    public SaveData currentSaveData = new SaveData();

    public const string saveDirectory = "/SaveData/";
    public const string fileName = "SaveGame.sav";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public bool SaveGame()
    {
        string _dir = Application.persistentDataPath + saveDirectory;

        if (!Directory.Exists(_dir))
            Directory.CreateDirectory(_dir);

        string _json = JsonUtility.ToJson(currentSaveData, prettyPrint : true);
        /*
         * prettyprint = true  -> �б� ����
         * prettyprint = false -> ���� �뷮 ����ȭ
         */

        File.WriteAllText(path: _dir + fileName, contents: _json);

        GUIUtility.systemCopyBuffer = _dir;
        Debug.Log($"Save ��� : {_dir}");
        // ���� ��θ� üũ�ϱ� ���ؼ�

        //�� �ε� ���� ��� ��� save�� �̻��� �� ������ �ִٸ� üũ�Ͽ� false�� ��ȯ����.
        
        return true;
    }

    public void LoadGame()
    {
        string fullPath = Application.persistentDataPath + saveDirectory + fileName;
        SaveData loadedData = new SaveData();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            loadedData = JsonUtility.FromJson<SaveData>(json);
        }

        else
        {
            Debug.LogError(message: "Save Data does not exist!");
        }

        currentSaveData = loadedData;
    }
}