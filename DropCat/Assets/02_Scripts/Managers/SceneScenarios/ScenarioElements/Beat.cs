using System.Collections;
using UnityEngine.Events;

interface IBeatStandard
{
    public void InitializeBeat();
    public void PlayBeat();
    public void StopBeat();
}

enum BeatStatus
{
    isLoading, isLocked, hasEntered, isRepeating, isFinished
}

public class Beat: IBeatStandard
{
    // Smallest element that composes one scenario event 
    // This can be a page of dialogue, a cutscene, etc.
    private UnityEvent beatTriggerEnter = null;
    private UnityEvent beatTriggerRepeat = null;
    private UnityEvent beatTriggerExit = null;

    private UnityAction exitTrigger = null;
    private UnityAction ExitTrigger { set { ExitTrigger = value; } }
    private BeatStatus currentStatus = BeatStatus.isLoading;

    public Beat()
    {
        InitializeBeat();
    }

    public virtual void InitializeBeat()
    {
        currentStatus = BeatStatus.isLoading;
        if (beatTriggerEnter == null)
        {
            beatTriggerEnter = new UnityEvent();
        }
        if (beatTriggerRepeat == null)
        {
            beatTriggerRepeat = new UnityEvent();
        }
        if(beatTriggerExit == null)
        {
            beatTriggerExit = new UnityEvent();
        }
    }

    public void AddEventsToBeatEnter(UnityAction eventMethod)
    {   
        UnLockBeat();
        if(currentStatus == BeatStatus.isLoading)
            beatTriggerEnter.AddListener(eventMethod);
        else
            throw new AttemptAddingToLockedScenario("Adding event to locked Beat is detected");
        LockBeat();
    }

    public void AddEventsToBeatRepeat(UnityAction eventMethod)
    {
        UnLockBeat();
        if (currentStatus == BeatStatus.isLoading)
            beatTriggerRepeat.AddListener(eventMethod);
        else
            throw new AttemptAddingToLockedScenario("Adding event to locked Beat is detected");
        LockBeat();
    }

    public void AddEventsToBeatExit(UnityAction eventMethod)
    {
        UnLockBeat();
        if (currentStatus == BeatStatus.isLoading)
            beatTriggerExit.AddListener(eventMethod);
        else
            throw new AttemptAddingToLockedScenario("Adding event to locked Beat is detected");
        LockBeat();
    }

    public virtual void PlayBeat()
    {
        for (int i = 0; i < 3; ++i)
        {
            PlaySingleBeat();
        }
    }

    public void PlaySingleBeat()
    {
        if (currentStatus == BeatStatus.isLocked)
        {
            beatTriggerEnter.Invoke();
            currentStatus = BeatStatus.hasEntered;
        }
        else if (currentStatus == BeatStatus.hasEntered)
        {
            beatTriggerRepeat.Invoke();
            currentStatus = BeatStatus.isRepeating;
        }
        else
            return;
    }

    private void LockBeat()
    {
        currentStatus = BeatStatus.isLocked;
    }

    private void UnLockBeat()
    {
        currentStatus = BeatStatus.isLoading;
    }

    public void StopBeat()
    {
        if(currentStatus == BeatStatus.isRepeating)
        {
            currentStatus = BeatStatus.isFinished;
            beatTriggerExit.Invoke();
        }
    }
}