using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance = null;

    public GameManager Game { get; private set; }
    public DataManager Data { get; private set; }
    public SceneManagerEX Scene { get; private set; }
    public InventoryManager Inventory { get; private set; }
    public SoundManager Sound { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Data = InitManager<DataManager>(Data);
            Scene = InitManager<SceneManagerEX>(Scene);
            Game = InitManager<GameManager>(Game);
            Inventory = InitManager<InventoryManager>(Inventory);
            Sound = InitManager<SoundManager>(Sound);
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
    /// �� ������ �������� ������Ʈ�� Resources �������� �ҷ��ɴϴ�.
    /// </summary>
    /// <param name="path">Resources ���� ���� ���</param>
    /// <param name="objName">������ obj �̸�</param>
    public GameObject InstantiateResouce(string path, string objName)
    {
        GameObject newObj = Instantiate(Resources.Load<GameObject>(path));
        newObj.name = objName;
        return newObj;
    }

    public void Clear()
    {
        Game.Clear();
        Data.Clear();
        Scene.Clear();
        Inventory.Clear();
        Sound.Clear();
    }
}

public interface IInitManager
{
    public void Init();
    public void Clear();
}