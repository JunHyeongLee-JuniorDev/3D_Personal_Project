using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleSwitch_UI : MonoBehaviour
{
    private Slider slider;

    private Image fill;

    [Header ("색상 설정")]
    [SerializeField]
    private Color onColor_Fill;
    [SerializeField]
    private Color offColor_Fill;

    [Header("버튼 이벤트")]
    public UnityEvent onToggle_Event;
    public UnityEvent offToggle_Event;

    [SerializeField]
    private float aniDuration;

    public bool isOn;

    private Coroutine coroutineCash;
    /*
     * 슬라이더의 값을 좌 우로 나눠야 하는데...
     * 이전 bool이 저기~ 라면 이 값을 저기~ 라면 이 값을
     */

    private void Start()
    {
        slider = GetComponent<Slider>();
        fill = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    public void OnClickAni()
    {

        if (coroutineCash != null) StopCoroutine(coroutineCash);
        coroutineCash = StartCoroutine(toggleSwitchAni());
    }

    private IEnumerator toggleSwitchAni()
    {
        float _endValue;
        Color _endColor;

        if (isOn)
        {
            _endValue = 0.0f;
            _endColor = offColor_Fill;
            offToggle_Event?.Invoke();
        }

        else
        {
            _endValue = 1.0f;
            _endColor = onColor_Fill;
            onToggle_Event?.Invoke();
        }
        isOn = !isOn;



        while (slider.value > _endValue + 0.1f ||
            slider.value < _endValue - 0.1f)
        {
            fill.color = Color.Lerp(fill.color, _endColor, aniDuration * Time.deltaTime);
            slider.value = Mathf.Lerp(slider.value, _endValue, aniDuration * Time.deltaTime);

            yield return null;
        }

        slider.value = _endValue;

        Debug.Log("코루틴 종료");
    }
}
