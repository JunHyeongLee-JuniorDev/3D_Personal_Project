using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    private float defaultTime;
    private float timeCounter;
    private float maxTime;
    private Coroutine coroutineCash;
    private MonoBehaviour controller;

    public bool isEnd { get; private set; }

    public Timer(float defaultTime, float maxTime, MonoBehaviour monoBehaviour)
    {
        this.defaultTime = defaultTime;
        this.maxTime = maxTime;
        controller = monoBehaviour;
        isEnd = false;
    }

    public void StartTimer()
    {
        if(coroutineCash != null)
            controller.StopCoroutine(coroutineCash);

        coroutineCash = controller.StartCoroutine(StartTimer_co());
    }

    private IEnumerator StartTimer_co()
    {
        isEnd = false;
        timeCounter = defaultTime;

        while (timeCounter < maxTime)
        {
            timeCounter += Time.deltaTime;
            yield return null;
        }
        timeCounter = defaultTime;
        isEnd = true;
    }
}
