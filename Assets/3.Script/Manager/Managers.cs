using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    public static Managers Instance = null;

    public GameManager Game { get; private set; }
    public DataManager Data { get; private set; }
    public SoundManager Sound { get; private set; }
    public SceneManagerEX Scene { get; private set; }
    public SpawnManager SpawnManager { get; private set; }

    public InventoryManager Inventory { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            //InitManager<SoundManager>(Sound);
            //InitManager<SpawnManager>(SpawnManager);
            //InitManager<InventoryManager>(Inventory);
            // 게임 씬으로 들어갈 때 맞는 데이터를 들고 온다.
            //SceneManager.activeSceneChanged += DynamicInitManager;

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Data = InitManager<DataManager>(Data);
            Scene = InitManager<SceneManagerEX>(Scene);
            Game = InitManager<GameManager>(Game);
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

                    Debug.Log(_manager.name + " : 생성 완료");   
            }
        }

        return _manager;
    }

    private void DynamicInitManager(Scene current, Scene next)
    {

    }
}

public interface IInitManager
{
    public void Init();
}