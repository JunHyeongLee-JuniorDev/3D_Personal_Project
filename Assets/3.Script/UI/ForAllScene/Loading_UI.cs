using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading_UI : MonoBehaviour
{
    private Slider m_slider;

    private void Start()
    {
        m_slider = GetComponent<Slider>();
        m_slider.value = 0.0f;
        StartCoroutine(ChangeSceneWithAsync_co(EScene.GAME));
    }

    private IEnumerator ChangeSceneWithAsync_co(EScene scene)
    {
        AsyncOperation sceneOP = SceneManager.LoadSceneAsync(GetSceneName(scene));
        sceneOP.allowSceneActivation = false;
        float fakeTimer = 0.0f;


        while (!sceneOP.isDone)
        {
            if (m_slider.value < 0.99f)
            {
                m_slider.value = Mathf.Lerp(m_slider.value, sceneOP.progress, Time.unscaledDeltaTime);
            }

            else
            {
                fakeTimer += Time.unscaledDeltaTime;
                m_slider.value = Mathf.Lerp(m_slider.value, 1.0f, fakeTimer);
            }

            if (m_slider.value < 1.0f)
            {
                sceneOP.allowSceneActivation = true;
                yield break;
            }


            yield return null;
        }

    }

    private string GetSceneName(EScene sceneType)
    {
        return System.Enum.GetName(typeof(EScene), sceneType);
    }
}
