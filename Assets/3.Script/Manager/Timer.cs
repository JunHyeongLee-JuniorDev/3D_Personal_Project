using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer 
{
    private float maxTime;
    private Coroutine coroutineCash;
    private MonoBehaviour controller;

    public bool isEnd { get; private set; }

    public Timer(float defaultTime, float maxTime, MonoBehaviour monoBehaviour)
    {
        this.maxTime = maxTime;
        controller = monoBehaviour;
        isEnd = true;
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
        yield return new WaitForSeconds(maxTime);
        isEnd = true;
    }
}
