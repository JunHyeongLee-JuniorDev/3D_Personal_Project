using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleSwitch_UI : MonoBehaviour
{
    private Slider slider;

    private Image fill;

    [Header ("���� ����")]
    [SerializeField]
    private Color onColor_Fill;
    [SerializeField]
    private Color offColor_Fill;

    [Header("��ư �̺�Ʈ")]
    public UnityEvent onToggle_Event;

    [SerializeField]
    private float aniDuration;

    public bool isOn;

    private Coroutine coroutineCash;
    /*
     * �����̴��� ���� �� ��� ������ �ϴµ�...
     * ���� bool�� ����~ ��� �� ���� ����~ ��� �� ����
     */

    private void Awake()
    {
        slider = GetComponent<Slider>();
        Debug.Log(slider);
        fill = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        Debug.Log(fill);
        AssignValueToSlider();
    }

    public void AssignValueToSlider()
    {
        if (isOn)
        {
            slider.value = 1.0f;
            fill.color = onColor_Fill;
        }

        else
        {
            slider.value = 0.0f;
            fill.color = offColor_Fill;
        }
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
        }

        else
        {
            _endValue = 1.0f;
            _endColor = onColor_Fill;
        }

        isOn = !isOn;
        onToggle_Event?.Invoke();



        while (slider.value > _endValue + 0.1f ||
            slider.value < _endValue - 0.1f)
        {
            fill.color = Color.Lerp(fill.color, _endColor, aniDuration * Time.deltaTime);
            slider.value = Mathf.Lerp(slider.value, _endValue, aniDuration * Time.deltaTime);

            yield return null;
        }
        
        fill.color = _endColor;
        slider.value = _endValue;
    }
}
