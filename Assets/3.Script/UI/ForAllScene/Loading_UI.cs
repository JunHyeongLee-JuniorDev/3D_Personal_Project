using System;
using System.Collections;
using TMPro;
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

        float realTimer = 0.0f;
        float fakeTimer = 0.0f;

        while (!sceneOP.isDone)
        {
            if (m_slider.value < 0.8f)
            {
                realTimer += Time.deltaTime;
                m_slider.value = Mathf.Lerp(m_slider.value, sceneOP.progress, realTimer);
            }

            else
            {
                fakeTimer += Time.deltaTime;
                m_slider.value = Mathf.Lerp(m_slider.value, 1.0f, fakeTimer);

                if (m_slider.value >= 1.0f)
                {
                    sceneOP.allowSceneActivation = true;
                }
            }

            yield return null;
        }

        yield break;
    }

    private string GetSceneName(EScene sceneType)
    {
        return System.Enum.GetName(typeof(EScene), sceneType);
    }
}
