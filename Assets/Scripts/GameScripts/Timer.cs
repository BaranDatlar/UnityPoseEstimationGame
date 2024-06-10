using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public static Timer instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }


    public void SetTimer(float delayTime, Action action)
    {
        StartCoroutine(TimerCoroutine(delayTime, action));
    }

    
    private IEnumerator TimerCoroutine(float delayTime, Action action)
    {
        yield return new WaitForSeconds(delayTime);
        action.Invoke();
    }

}
