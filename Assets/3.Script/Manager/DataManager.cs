using UnityEngine;
using System;
using System.IO;

public class DataManager : MonoBehaviour//, IInitManager
{
    public static DataManager instance = null;
    public static SaveData currentSaveData = new SaveData();

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

    public bool Save()
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

        GUIUtility.systemCopyBuffer = _dir + fileName;

        return true;
    }
}