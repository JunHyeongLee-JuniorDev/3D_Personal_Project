using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class FadeInOut_UI : MonoBehaviour
{
    /*
     * ÄÓ ¶§ ±×¸®°í ²ø ¶§ Èñ¹ÌÇØÁö±â 
     */

    private CanvasGroup group;
    private Tween tween;

    protected virtual void Awake()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0.0f;
    }

    protected virtual void OnEnable()
    {
        tween?.Kill();

        tween = group.DOFade(1.0f, 0.5f).SetEase(Ease.OutQuad);
    }

    public void OnClickFade_Btn()
    {
        tween?.Kill();

        tween = group.DOFade(0.0f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        tween?.Kill();
        group.alpha = 0.0f;
    }
}
