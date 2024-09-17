using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TextHyperLink_UI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea(2,4)]
    [SerializeField] private string URL;
    [SerializeField] private Color changeColor;

    private Color originColor;
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        originColor = text.color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Application.OpenURL(URL);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = changeColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = originColor;
    }
}
