using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedState_UI : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private Selectable selectable = null;

    private void Start()
    { 
        selectable = GetComponent<Selectable>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectable.Select();
    }
}
