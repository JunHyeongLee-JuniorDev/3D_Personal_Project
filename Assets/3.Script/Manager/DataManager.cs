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
         * prettyprint = true  -> 읽기 쉽게
         * prettyprint = false -> 파일 용량 최적화
         */

        File.WriteAllText(path: _dir + fileName, contents: _json);

        GUIUtility.systemCopyBuffer = _dir;
        Debug.Log($"Save 경로 : {_dir}");
        // 직접 경로를 체크하기 위해서

        //씬 로드 같은 경우 등등 save에 이상이 갈 사항이 있다면 체크하여 false를 반환하자.
        
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