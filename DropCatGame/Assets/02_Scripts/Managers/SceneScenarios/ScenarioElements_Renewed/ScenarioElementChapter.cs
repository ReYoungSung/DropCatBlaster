using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinearScenarioStructure
{
    public class ScenarioElementChapter : MonoBehaviour
    {
        private Queue<ScenarioElementSequence> currentSequenceQueue = null;
        public int GetCurrentCount { get { return currentSequenceQueue.Count; } }
        private ScenarioElementSequence currentSequencePlaying = null;
        public ScenarioElementSequence CurrentSequencePlaying { get { return currentSequencePlaying; } }
        private Coroutine chapterCoroutine = null;

        private void Awake()
        {
            if (currentSequenceQueue == null)
            {
                currentSequenceQueue = new Queue<ScenarioElementSequence>();
            }
        }

        public void LevelLoadRoutine(Action levelLoadingAction)
        {
            if (currentSequenceQueue.Count <= 0)
            {
                if (levelLoadingAction != null)
                    levelLoadingAction.Invoke();
                else
                    Debug.Log("EMPTY LEVEL LOADING ACTION");
            }
            else
            {
                Debug.Log("nOT EMPTY LEVELROUTINE");
            }
        }

        private void Update()
        {
            if (currentSequencePlaying == null)
                AllocateNewSequence();
            if(currentSequencePlaying != null)
            {
                if (currentSequencePlaying.CurrentBeatPlaying == null || currentSequencePlaying.CurrentBeatPlaying.IsFinished)
                {
                    currentSequencePlaying.PlaySequenceElement();
                }
                chapterCoroutine = StartCoroutine(currentSequencePlaying.CurrentBeatPlaying.PlayBeatRoutine());
                if (currentSequencePlaying.GetCurrentCount <= 0 && currentSequencePlaying.CurrentBeatPlaying.IsFinished)
                {
                    currentSequencePlaying = null;
                }
            }
        }

        public void AllocateNewSequence()
        {
            if(currentSequencePlaying != null)
            {
                if (!currentSequencePlaying.IsFinished)
                    currentSequencePlaying = currentSequenceQueue.Dequeue();
            }
            else
            {
                if(0 < currentSequenceQueue.Count)
                    currentSequencePlaying = currentSequenceQueue.Dequeue();
            }
        }

        public void AddSequenceToCurrent(ScenarioElementSequence sequence)
        {
            currentSequenceQueue.Enqueue(sequence);
        }
    }
}