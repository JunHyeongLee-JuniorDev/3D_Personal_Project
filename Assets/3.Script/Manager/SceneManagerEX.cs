using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX : MonoBehaviour, IInitManager
{
    public BaseScene currentScene { get { return FindObjectOfType<BaseScene>(); } }
    public Animator fadeBG { get; private set; }
    public void Init()
    {
        fadeBG = Instantiate(Resources.Load<GameObject>("Prefabs/UI/FadeInOut")).GetComponentInChildren<Animator>();
        fadeBG.transform.parent.SetParent(transform, false);
    }

    public string GetSceneName(EScene sceneType)
    {
        return System.Enum.GetName(typeof(EScene), sceneType);
    }

    public void ChangeScene(EScene scene)
    {
        StartCoroutine(ChangeScene_co(scene));
    }

    private IEnumerator ChangeScene_co(EScene scene)
    {
        fadeBG.Play("CrossFade End");
        Managers.Instance.Clear();
        yield return new WaitForSeconds(1.0f);
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
    GAME,
    LOADING
};
