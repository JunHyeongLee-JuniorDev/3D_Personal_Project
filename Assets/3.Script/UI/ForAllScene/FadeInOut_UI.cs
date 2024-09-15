using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FadeInOut_UI : MonoBehaviour
{
    /*
     * ÄÓ ¶§ ±×¸®°í ²ø ¶§ Èñ¹ÌÇØÁö±â 
     */

    private CanvasGroup group;
    private Tween tween;

    private void Start()
    {
        group = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        tween?.Kill();
        tween = group.DOFade(1.0f, 1.0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))// For Debug
        {
            OnClickFade_Btn();
        }
    }

    public void OnClickFade_Btn()
    {
        tween?.Kill();
        tween = group.DOFade(0.0f, 1.0f).OnComplete(() => gameObject.SetActive(false));
    }
}
