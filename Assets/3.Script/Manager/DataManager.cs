using UnityEngine;
using System;
using System.IO;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class DataManager : MonoBehaviour, IInitManager
{
    //Item DataBase
    public ItemDataBase itemDataBase { get; private set; }

    //Save Files
    public GameSaveData[] currentSaveData;
    public SettingSaveData settingSaveData;
    public AudioMixer audioMixer;

    //현재 세이브 파일 index
    public int currentSaveIndex;

    // 디볼트 세이브 데이터 
    private SettingSaveData defaultSetting = new SettingSaveData();

    public MonsterAniDataBase monsterAniDB { get; private set; }
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
        itemDataBase = new ItemDataBase();
        itemDataBase.LoadDBFromJson();
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
        monsterAniDB = new MonsterAniDataBase();
        monsterAniDB.init();
    }

    public void Clear()
    {

    }

    private void Start()
    {
        audioMixer.SetFloat("Master", settingSaveData.masterVolume);
        audioMixer.SetFloat("Music", settingSaveData.musicVolume);
        audioMixer.SetFloat("SFX", settingSaveData.SFXVolume);
    }

    /// <summary>
    /// 지정한 파일 인덱스로 저장
    /// </summary>
    /// <param name="saveFileIndex"></param>
    /// <returns>저장 성공시 true를 반환</returns>
    public bool SaveGame(int saveFileIndex)
    {
        string _dir = Application.persistentDataPath + saveDirectory;

        if (!Directory.Exists(_dir))
            Directory.CreateDirectory(_dir);

        string _json = JsonUtility.ToJson(currentSaveData[saveFileIndex], prettyPrint : true);
        /*
         * 
         * prettyprint = true  -> 읽기 쉽게
         * prettyprint = false -> 파일 용량 최적화
         */

        File.WriteAllText(path: _dir + gameSaveNames[saveFileIndex], contents: _json);

        GUIUtility.systemCopyBuffer = _dir;
        Debug.Log($"Save 경로 : {_dir}");
        // 직접 경로를 체크하기 위해서

        //씬 로드 같은 경우 등등 save에 이상이 갈 사항이 있다면 체크하여 false를 반환하자.
        
        return true;
    }

    /// <summary>
    /// ID로 찾음 없으면 각 인스턴스에서 새로 만듬
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public monsterSaveData LoadMonsterData(int ID, monsterSaveData newMonster)
    {
        foreach (monsterSaveData monster in currentSaveData[currentSaveIndex].monsterSaveDatas)
        {
            if (monster.monsterID == ID)
                return monster;
        }

        currentSaveData[currentSaveIndex].monsterSaveDatas.Add(newMonster);

        return newMonster;
    }

    public void SavePlayerPosition()
    {
        currentSaveData[currentSaveIndex].savePlayerData.playerPosition = 
            Managers.Instance.Game.playerController.transform.position;

        currentSaveData[currentSaveIndex].savePlayerData.playerRotation = 
            Managers.Instance.Game.playerController.transform.rotation;
    }

    public void CreateNewGame(int saveFileIndex, string userName)
    {
        ItemData initHealPotion = itemDataBase.GetItemData("체력 포션");
        ItemData initManaPotion = itemDataBase.GetItemData("마나 포션");
        GameSaveData _newGamedata = new GameSaveData();
        _newGamedata.savePlayerData = new PlayerData(new InventorySlot(initHealPotion, initHealPotion.updatableStack),
                                                     new InventorySlot(initManaPotion, initManaPotion.updatableStack));
        _newGamedata.savePlayerData.name = userName;

        //스텟 초기화
        _newGamedata.savePlayerData.currentHealth = 200f;
        _newGamedata.savePlayerData.maxHealth = 200f;
        _newGamedata.savePlayerData.currentMana = 100f;
        _newGamedata.savePlayerData.maxMana = 100f;
        _newGamedata.savePlayerData.currentStamina = 100f;
        _newGamedata.savePlayerData.maxStamina = 100f;
        _newGamedata.savePlayerData.staminaStat = 1;
        _newGamedata.savePlayerData.strengthStat = 1;
        _newGamedata.savePlayerData.lifeStat = 1;
        _newGamedata.savePlayerData.manaStat = 1;
        _newGamedata.savePlayerData.level = 1;
        _newGamedata.savePlayerData.magicDamage = 20f;
        _newGamedata.savePlayerData.weaponDamage = 25f;
        _newGamedata.savePlayerData.wallet = 0;
        _newGamedata.savePlayerData.playerPosition = Resources.Load<PlayerSO>("ScriptableObj/Player/PlayerData").GroundedData.defaultSpawnPosition;
        _newGamedata.savePlayerData.playerRotation = Resources.Load<PlayerSO>("ScriptableObj/Player/PlayerData").GroundedData.defaultSpawnRotation;
        _newGamedata.savePlayerData.spawnPosition = _newGamedata.savePlayerData.playerPosition;
        
        currentSaveData[saveFileIndex] = _newGamedata;
        SaveGame(saveFileIndex);
    }

    /// <summary>
    /// 게임 세이브 파일을 인덱스로 받아서 부름
    /// </summary>
    /// <param name="fileIndex">데이터 메니저의 파일 이름 string index로 파일을 찾음</param>
    /// <returns>디렉토리에 있는 파일을 return 없을 시 default 파일 return</returns>
    public void LoadGame(int fileIndex)
    {
        string fullPath = Application.persistentDataPath + saveDirectory + gameSaveNames[fileIndex];

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(json);
            currentSaveData[fileIndex] = loadedData;
            currentSaveData[fileIndex].savePlayerData.currentPotion = 
                currentSaveData[fileIndex].savePlayerData.equipments[(int)EEquipmentType.Heal];
        }

        else
        {
            Debug.Log(message: "Save Data does not exist!");
            currentSaveData[fileIndex] = null;
        }
    }

    public BornFireData AssignBornFireData(string fireName)
    {
        foreach (BornFireData data in currentSaveData[currentSaveIndex].bornFireDatas)
        {
            if(data.fireName == fireName)
                return data;
        }

        Debug.LogWarning("화톳불 데이터 불러오기 실패");
        return null;
    }


    /// <summary>
    /// 세팅 파일을 디렉토리에서 부름
    /// </summary>
    /// <returns>디렉토리에 있는 파일을 return 없을 시 default 파일을 return</returns>
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
        Debug.Log($"세팅 저장 경로 {_dir}");

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
            Debug.Log("해당 파일이 없습니다.");
        }
    }
}