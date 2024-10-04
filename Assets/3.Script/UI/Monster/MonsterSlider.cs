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
        Vector3 dir = Camera.main.transform.position - transform.position;

        rectTransform.localScale = Vector3.one * dir.magnitude * 0.5f;
        transform.LookAt(Camera.main.transform.position);
    }

}
