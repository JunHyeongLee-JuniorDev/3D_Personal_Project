using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovePanel : MonoBehaviour
{
    [Header("Set ease")]
    [SerializeField]
    private Ease ease;

    [Header("Positions")]
    [SerializeField]
    private Vector2 spawnPos;

    [SerializeField]
    private Vector2 startPos;
    [SerializeField]
    private float startDuration;

    [SerializeField]
    private Vector2 endPos;
    [SerializeField]
    private float endDuration;

    private RectTransform rectTransform;
    private Tween tween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        GotoStartPos();
    }

    private void OnDisable()
    {
        rectTransform.anchoredPosition = spawnPos;
    }

    private void GotoStartPos()
    {
            tween?.Kill();

        tween = rectTransform.DOAnchorPos(startPos, startDuration).SetEase(ease);
    }

    public void GotoEndPos()
    {
            tween?.Kill();

        tween = rectTransform.DOAnchorPos(endPos, endDuration).SetEase(ease).
            OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
