using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePopUp : MonoBehaviour
{
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI description;
    private CanvasGroup canvasGroup;

    private Tween tweenCash;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        tweenCash?.Kill();
        canvasGroup.DOFade(0.8f, 1.0f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    public void ChangeText(string nextText)
    {
        description.text = nextText;
    }

    public void ChangeIcon(Sprite icon)
    {
        Icon.sprite = icon;
        Icon.color = Color.white;
    }

    public void TurnOffIcon()
    {
        Icon.sprite = null;
        Icon.color = Color.clear;
    }

    private void OnDisable()
    {
        canvasGroup.alpha = 1.0f;
        tweenCash?.Kill();
    }
}
