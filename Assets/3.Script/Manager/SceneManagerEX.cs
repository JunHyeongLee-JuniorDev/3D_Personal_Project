using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX : MonoBehaviour
{
    public void ChangeScene(EScene scene)
    {
        switch (scene)
        {
            case EScene.TITLE:
                break;

            case EScene.SAMPLE:
                break;
        }
        SceneManager.LoadScene((int)scene);
    }
}

public enum EScene
{
    TITLE,
    SAMPLE
};
