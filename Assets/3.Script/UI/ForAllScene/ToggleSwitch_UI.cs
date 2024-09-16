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
    public UnityEvent offToggle_Event;

    [SerializeField]
    private float aniDuration;

    public bool isOn;

    private Coroutine coroutineCash;
    /*
     * �����̴��� ���� �� ��� ������ �ϴµ�...
     * ���� bool�� ����~ ��� �� ���� ����~ ��� �� ����
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

        Debug.Log("�ڷ�ƾ ����");
    }
}
