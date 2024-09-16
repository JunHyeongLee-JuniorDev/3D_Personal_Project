using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItle_UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject settings_UI;

    [SerializeField]
    private GameObject more_UI;

    public void OnClickPlay()
    {
        Managers.Instance.Scene.ChangeScene(EScene.PLAYER);
    }

    public void OnClickSettings()
    {
        settings_UI.SetActive(true);
    }

    public void OnClickMore()
    {
        more_UI.SetActive(true);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}
