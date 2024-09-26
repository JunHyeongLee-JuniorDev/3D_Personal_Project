using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Timer 
{
    private float maxTime;
    public Coroutine coroutineCash { get; private set; }
    private MonoBehaviour controller;
    private Action OnComplete;

    public bool isTickin { get; private set; }

    public Timer(float maxTime, MonoBehaviour monoBehaviour)
    {
        this.maxTime = maxTime;
        controller = monoBehaviour;
        isTickin = false;
    }

    public void UpdateMaxTime(float maxTime)
    {
        this.maxTime = maxTime;
    }

    public void StartTimer(Action onComplete)
    {
        isTickin = true;
        this.OnComplete = onComplete;
        coroutineCash = controller.StartCoroutine(StartTimer_co(onComplete));
    }

    private IEnumerator StartTimer_co(Action onComplete)
    {
        yield return new WaitForSeconds(maxTime);
        this.OnComplete?.Invoke();
        isTickin = false;
    }

    public void StopTimer()
    {
        if (coroutineCash != null)
        {
            isTickin = false;
            this.OnComplete?.Invoke();
            controller.StopCoroutine(coroutineCash);
        }
    }
}
