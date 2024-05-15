using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class IntervalBreak : Beat, IBeatStandard
{
    private UnityEvent intervalBreak = null;
    private float timeInSeconds = 0;
    public float TimeInSeconds { get { return timeInSeconds; } set { timeInSeconds = value; } }

    public override void InitializeBeat()
    {
        if (intervalBreak == null)
        {
            intervalBreak = new UnityEvent();
        }
    }

    public override void PlayBeat()
    {
        if (intervalBreak != null)
            intervalBreak.Invoke();
    }

    public IEnumerator HalfForBreak(float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
    }
}
