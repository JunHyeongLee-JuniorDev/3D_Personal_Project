using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    private float maxTime;
    public Coroutine coroutineCash { get; private set; }
    private MonoBehaviour controller;

    public bool isEnd { get; private set; }

    public Timer(float maxTime, MonoBehaviour monoBehaviour)
    {
        this.maxTime = maxTime;
        controller = monoBehaviour;
        isEnd = true;

    }

    public void UpdateMaxTime(float maxTime)
    {
        this.maxTime = maxTime;
    }

    public void StartTimer()
    {
        if(coroutineCash != null)
            controller.StopCoroutine(coroutineCash);
        isEnd = false;
        coroutineCash = controller.StartCoroutine(StartTimer_co());
    }

    private IEnumerator StartTimer_co()
    {
        yield return new WaitForSeconds(maxTime);
        isEnd = true;
    }
}
