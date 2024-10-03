using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSlider : FakeSlider_UI
{
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        rectTransform.localScale = (transform.position - Camera.main.transform.position) * 0.1f;
        transform.LookAt(Camera.main.transform.position);
    }
}
