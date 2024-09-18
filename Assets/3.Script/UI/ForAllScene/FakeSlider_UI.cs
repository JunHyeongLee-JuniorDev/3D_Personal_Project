using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FakeSlider_UI : MonoBehaviour
{
    [SerializeField] private Slider m_slider;
    [SerializeField] private Image m_fakeImage;

    private Tween realTweenCash;
    private Tween fakeTweenCash;
    private Coroutine coroutineCash;

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

    public void ReduceSlider(float endValue, float maxValue)
    {
        realTweenCash?.Kill();
        realTweenCash = m_slider.DOValue(endValue / maxValue, duration).SetEase(Ease.OutExpo);
    }

    public void ReduceSliderWithFake(float endValue, float maxValue)
    {
        if (coroutineCash != null)
        StopCoroutine(coroutineCash);

        float currentValue = endValue/maxValue;


        fakeTweenCash?.Kill();
        fakeTweenCash = m_fakeImage.DOFillAmount(currentValue, duration);

        coroutineCash = StartCoroutine(ReduceFake_co(currentValue));
    }

    private IEnumerator ReduceFake_co(float currentValue)
    {
        yield return new WaitForSeconds(1.5f);

        realTweenCash?.Kill();
        realTweenCash = m_slider.DOValue(currentValue, duration).SetEase(Ease.OutExpo);
    }

}
