using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;

interface ISequenceStandard
{
    public void AssignBeatToCurrent(Beat beat, int index);
    public void PlaySequence();
}

public enum SequenceStatus
{
    isLoading, isLocked, isPending, isPlaying, isFinished
}

public class Sequence
{
    private string sequenceName = null;
    public string SequenceName { get { return sequenceName; } }
    private Queue<Beat> current = null;
    public int GetCurrentCount { get { return current.Count; } }
    private Beat currentBeatPlaying = null;
    private EndOfSequence EOS = null;
    private SequenceStatus currentStatus = SequenceStatus.isLoading;
    public SequenceStatus CurrentStatus { get { return currentStatus; } set { currentStatus = value; } }
    public bool IsFinished { get { return currentStatus == SequenceStatus.isFinished; } }

    public Sequence(string sequenceN)
    {
        this.sequenceName = sequenceN;
        InitiaizeSequence();
        InitializeTransitionTrigger();
    }

    private void InitiaizeSequence()
    {
        currentStatus = SequenceStatus.isLoading;
        if (current == null)
        {
            current = new Queue<Beat>();
        }
        EOS = new EndOfSequence();
    }

    private void InitializeTransitionTrigger()
    {
        currentStatus = SequenceStatus.isLoading;
    }

    public void AddBeatToCurrent(Beat beat)
    {
        if (currentStatus == SequenceStatus.isLoading)
            current.Enqueue(beat);
        else
            throw new AttemptAddingToLockedScenario("Adding Beat to locked Sequence is detected" + current.Count);
    }

    public void AddBehaviourToEOS(UnityAction eventMethod)
    {
        if (currentStatus == SequenceStatus.isLoading)
            EOS.AddEventsToEndOfSequence(eventMethod);
        else
            throw new AttemptAddingToLockedScenario("Adding EOS Event to locked Sequence is detected");
    }

    public void StartSequence()
    {
        if(currentStatus == SequenceStatus.isLocked)
        {
            PlaySequenceElement();
            currentStatus = SequenceStatus.isPlaying;
        }
        else
        {
            throw new AttemptPlayingUnLockedScenario("Starting Unlocked Sequence is detected");
        }
    }

    public void LockSequence()
    {
        AddBeatToCurrent(EOS);
        currentStatus = SequenceStatus.isLocked;
    }

    public void ContinueSequence()
    {
        if (currentStatus == SequenceStatus.isPlaying)
        {
            // Add conditions here
            currentBeatPlaying.StopBeat();
            PlaySequenceElement();

            if (currentBeatPlaying.Equals(EOS))
            {
                FinishSequence();
            }
        }
    }

    private void PlaySequenceElement()
    {
        currentBeatPlaying = current.Dequeue();
        currentBeatPlaying.PlayBeat();
    }

    private void FinishSequence()
    {
        currentStatus = SequenceStatus.isFinished;
    }
}