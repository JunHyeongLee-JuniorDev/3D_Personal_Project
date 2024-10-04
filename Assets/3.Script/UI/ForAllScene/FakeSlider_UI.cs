using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FakeSlider_UI : MonoBehaviour
{
    [SerializeField] protected Slider m_slider;
    [SerializeField] protected Image m_fakeImage;

    private Tween realTweenCash;
    private Tween fakeTweenCash;
    private Coroutine coroutineCash;
    public Canvas canvas { get; private set; }

    private float duration = 1.0f;

    //슬라이더의 max value를 초기화 할 시 유용할 방법
    //private void Start()
    //{
    //    Vector2 rectSize = m_slider.GetComponent<RectTransform>().sizeDelta;
    //    
    //    rectSize.x = 100f;
    //    
    //    m_slider.GetComponent<RectTransform>().sizeDelta = rectSize;
    //}

    public void AssignCanvas(Canvas canvas)
    {
        this.canvas = canvas;
    }

    public void ChangeWithoutTween(float endValue, float maxValue)
    {
        float currentValue = endValue / maxValue;
        m_slider.value = currentValue;
        m_fakeImage.fillAmount = currentValue;
    }

    public void ChangeSlider(float endValue, float maxValue)
    {
        realTweenCash?.Kill();
        realTweenCash = m_slider.DOValue(endValue / maxValue, duration).SetEase(Ease.OutExpo);
    }

    public void RefillWithFakeSlider(float endValue, float maxValue)
    {
        fakeTweenCash?.Kill();

        float currentValue = endValue / maxValue;

        fakeTweenCash = m_fakeImage.DOFillAmount(currentValue, duration).OnComplete(() =>
        {
            m_fakeImage.fillAmount = currentValue;
        });
    }

    public void ChangeSliderWithFake(float endValue, float maxValue)
    {
        if (coroutineCash != null)
        StopCoroutine(coroutineCash);

        float currentValue = endValue/maxValue;


        fakeTweenCash?.Kill();
        fakeTweenCash = m_fakeImage.DOFillAmount(currentValue, duration);

        coroutineCash = StartCoroutine(ChangeSlider_co(currentValue));
    }

    private IEnumerator ChangeSlider_co(float currentValue)
    {
        yield return new WaitForSeconds(1.0f);

        realTweenCash?.Kill();
        realTweenCash = m_slider.DOValue(currentValue, duration).SetEase(Ease.OutExpo);
    }

}
