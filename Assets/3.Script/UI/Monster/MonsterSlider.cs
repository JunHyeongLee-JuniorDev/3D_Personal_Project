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
        rectTransform.localScale = Vector3.one * (transform.position - Camera.main.transform.position).magnitude * 0.5f;
        transform.LookAt(Camera.main.transform.position);
    }
}
