using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading_UI : MonoBehaviour
{
    private Slider m_slider;

    private void Awake()
    {
        m_slider = GetComponent<Slider>();
        m_slider.value = 0.0f;
    }

    private void Start()
    {
        ChangeSceneWithAsync(EScene.GAME);
    }

    public void ChangeSceneWithAsync(EScene scene)
    {
        StartCoroutine(ChangeSceneWithAsync_co(scene));
    }

    private IEnumerator ChangeSceneWithAsync_co(EScene scene)
    {
        AsyncOperation sceneOP = SceneManager.LoadSceneAsync(GetSceneName(scene));
        sceneOP.allowSceneActivation = false;
        float realTimer = 0.0f;
        float fakeTimer = 0.0f;


        while (!sceneOP.isDone)
        {
            if (m_slider.value < 0.9f)
            {
                realTimer += Time.unscaledDeltaTime * 0.1f;
                m_slider.value = Mathf.Lerp(m_slider.value, sceneOP.progress, realTimer);
            }

            else
            {
                fakeTimer += Time.unscaledDeltaTime;
                m_slider.value = Mathf.Lerp(m_slider.value, 1.0f, fakeTimer);

                if (m_slider.value >= 0.99f)
                {
                    sceneOP.allowSceneActivation = true;
                    yield break;
                }
            }

            Debug.Log("코루틴 돌아가는 중");

            yield return null;
        }
    }

    private string GetSceneName(EScene sceneType)
    {
        return System.Enum.GetName(typeof(EScene), sceneType);
    }
}
