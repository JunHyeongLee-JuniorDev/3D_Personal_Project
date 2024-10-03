using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeOutPopUp_UI : FadeInOut_UI
{
    [SerializeField] private Material diedMaterial;
    [SerializeField] private Material bornFireMaterial;

    private TextMeshProUGUI text;
    private Timer fadeOutTimer;

    protected override void Awake()
    {
        base.Awake();
        text = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        fadeOutTimer = new Timer(2.0f, this);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        fadeOutTimer.StartTimer(() =>
        {
            OnClickFade_Btn();
        });
    }
}
