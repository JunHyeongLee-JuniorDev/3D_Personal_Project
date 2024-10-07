using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance = null;

    public GameManager Game { get; private set; }
    public DataManager Data { get; private set; }
    public SceneManagerEX Scene { get; private set; }
    public SpawnManager SpawnManager { get; private set; }
    public InventoryManager Inventory { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            //InitManager<SoundManager>(Sound);
            //InitManager<SpawnManager>(SpawnManager);
            // 게임 씬으로 들어갈 때 맞는 데이터를 들고 온다.
            //SceneManager.activeSceneChanged += DynamicInitManager;

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Data = InitManager<DataManager>(Data);
            Scene = InitManager<SceneManagerEX>(Scene);
            Game = InitManager<GameManager>(Game);
            Inventory = InitManager<InventoryManager>(Inventory);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private T InitManager<T>(T _manager) where T : MonoBehaviour, IInitManager
    {
        if (_manager == null)
        {
            _manager = GameObject.FindObjectOfType<T>();

            if (_manager == null)
            {
                    _manager = new GameObject(typeof(T).Name).AddComponent<T>();
                    _manager.transform.parent = transform;
                    _manager.Init();
            }
        }

        return _manager;
    }

    /// <summary>
    /// 씬 내에서 만들어야할 오브젝트를 Resources 폴더에서 불러옵니다.
    /// </summary>
    /// <param name="path">Resources 폴더 내의 경로</param>
    /// <param name="objName">생성할 obj 이름</param>
    public GameObject InstantiateResouce(string path, string objName)
    {
        GameObject newObj = Instantiate(Resources.Load<GameObject>(path));
        newObj.name = objName;
        return newObj;
    }
}

public interface IInitManager
{
    public void Init();
}