using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX : MonoBehaviour, IInitManager
{
    public BaseScene currentScene { get { return FindObjectOfType<BaseScene>(); } }
    public void Init()
    {

    }

    public string GetSceneName(EScene sceneType)
    {
        return System.Enum.GetName(typeof(EScene), sceneType);
    }

    public void ChangeScene(EScene scene)
    {
        Managers.Instance.Game.UIGroups.Clear();

        SceneManager.LoadScene(GetSceneName(scene));
    }

    public void Clear()
    {
        currentScene.Clear();
    }
}

public enum EScene
{
    UNKNOWN,
    TITLE,
    GAME
};
