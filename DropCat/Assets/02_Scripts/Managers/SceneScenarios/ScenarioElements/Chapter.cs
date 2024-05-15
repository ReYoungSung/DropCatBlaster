using System.Collections.Generic;
using UnityEngine;

public class Chapter
{
    public enum ChapterStatus
    {
        isLoading, isLocked, isPlaying, isFinished
    }

    private Queue<Sequence> current = null;
    public int GetCurrentCount { get { return current.Count; } }
    private Sequence currentSequencePlaying = null;
    public Sequence CurrentSequencePlaying { get { return currentSequencePlaying; } }
    private ChapterStatus currentStatus = ChapterStatus.isLoading;
    public ChapterStatus CurrentStatus { get { return currentStatus; } }

    public Chapter()
    {
        currentStatus = ChapterStatus.isLoading;
        if (current == null)
        {
            current = new Queue<Sequence>();
        }
    }

    public void ValidateChapter()
    {
        if(ChapterIsCompleted())
        {
            currentStatus = ChapterStatus.isFinished;
        }
    }

    private bool ChapterIsCompleted()
    {
        if (current.Count < 1)
        {
            if (currentSequencePlaying.IsFinished)
            {
                return true;
            }
            else
            {
                Debug.Log("Chapter queue is empty, but EOS is not active");
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void AddSequenceToCurrent(Sequence sequence)
    {
        if (currentStatus == ChapterStatus.isLoading)
        {
            sequence.LockSequence();
            current.Enqueue(sequence);
        }
        else
            throw new AttemptAddingToLockedScenario("Adding Sequence to locked Chapter is detected");
    }

    public void StartChapter()
    {
        LockChapter();
        if (currentStatus == ChapterStatus.isLocked)
        {
            PlayChapterElement();
            currentStatus = ChapterStatus.isPlaying;
        }
        else
        {
            throw new AttemptPlayingUnLockedScenario("Starting Unlocked Chapter is detected");
        }
    }

    private void PlayChapterElement()
    {
        if(current.Count != 0)
        {
            currentSequencePlaying = current.Dequeue();
            currentSequencePlaying.StartSequence();
        }
    }

    private void LockChapter()
    {
        currentStatus = ChapterStatus.isLocked;
    }

    public virtual void ContinueChapter()
    {
        if (currentSequencePlaying.IsFinished)
        {
            // Add conditions
            PlayChapterElement();
        }
    }
}