using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX : MonoBehaviour, IInitManager
{
    public void Init()
    {

    }
    public void ChangeScene(EScene scene)
    {
        Managers.Instance.Game.UIGroups.Clear();
        switch (scene)
        {
            case EScene.TITLE:
                break;

            case EScene.PLAYER:
                break;
        }
        SceneManager.LoadScene((int)scene);
    }

}

public enum EScene
{
    TITLE,
    PLAYER
};
