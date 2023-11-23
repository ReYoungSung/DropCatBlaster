using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public enum TimerMode
    {
        // TimeScale = 0 �̾ ����
        unscaledDeltaTime, 
        // TimeScale = 0 �̸� ������ ����
        scaledDeltaTime
    }

    public static Timer instance;
    public event Action timeUpTriggerEnter;

    private bool timerTicking = false;

    private float triggerTime = 5f;
    public float TriggerTime { get { return triggerTime; } set { triggerTime = value; } }
    private float timeLeft;

    [SerializeField] private TimerMode timerMode = TimerMode.unscaledDeltaTime;
    public TimerMode Mode { get { return timerMode; } set { timerMode = value; } }
    private Coroutine coroutine = null;

    private void Awake()
    {
        instance = this;   
    }

    private void Update()
    {
        if(timerTicking == false)
        {
            coroutine = null;
        }
        else
        {
            TimerRoutine();
        }
    }

    public void StartTicking()
    {
        if (!timerTicking)
        {
            timeLeft = triggerTime;
            timerTicking = true;
        }
    }

    private void TimerRoutine()
    {
        if (timerMode == TimerMode.unscaledDeltaTime)
        {
            Ticks(Time.unscaledDeltaTime);
        }
        else if (timerMode == TimerMode.scaledDeltaTime)
        {
            Ticks(Time.deltaTime);
        }
    }

    private void Ticks(float deltaTime)
    {
        if (timeLeft > 0)
        {
            timeLeft -= deltaTime;
        }
        else if (timeLeft <= 0)
        {
            StopTicking();
            if(timeUpTriggerEnter != null)
            {
                timeUpTriggerEnter.Invoke();
                Debug.Log("Timer REloadScene");
            }
        }
    }

    public void StopTicking()
    {
        if(timerTicking)
        {
            timeLeft = 0;
            timerTicking = false;
        }
    }

    public IEnumerator CoroutineTimer(Beat currentBeat)
    {
        while (timerTicking)
        {
            yield return new WaitForSeconds(triggerTime);
            timerTicking = false;
            currentBeat.StopBeat();
        }
    }

    public IEnumerator CoroutineTimer(float triggerT, UnityAction response)
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(triggerT);
            timerTicking = false;
            response.Invoke();
            yield break;
        }
        
    }

    private void OnEnable()
    {
        timeLeft = triggerTime;
        timerTicking = false;
    }

    private void OnDisable()
    {
        triggerTime = 0;
        timerTicking = false;
    }
}
