using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Managers : MonoBehaviour
{
    public static Managers Instance = null;

    public GameManager Game { get; private set; }
    public DataManager Data { get; private set; }
    public SoundManager Sound { get; private set; }
    public SceneManagerEX Scene { get; private set; }
    public SpawnManager SpawnManager { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<Managers>();

            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                InitManager<GameManager>(Game);
                InitManager<DataManager>(Data);
                InitManager<SoundManager>(Sound);
                InitManager<SceneManagerEX>(Scene);
                InitManager<SpawnManager>(SpawnManager);
            }
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void InitManager<T>(T _manager) where T : MonoBehaviour
    {
        if (_manager == null)
        {
            _manager = GameObject.FindObjectOfType<T>();

            if (_manager == null)
            {
                try
                {
                    _manager = new GameObject(typeof(T).Name).AddComponent<T>();
                    _manager.transform.parent = transform;

                    Debug.Log(_manager.name + " : 생성 완료");
                }

                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }

}

public interface IInitManager
{
    void Init();
}