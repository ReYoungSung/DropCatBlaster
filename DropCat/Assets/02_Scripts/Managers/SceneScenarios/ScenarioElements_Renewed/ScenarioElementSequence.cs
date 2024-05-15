using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinearScenarioStructure
{
    public class ScenarioElementSequence
    {
        private string sequenceName = null;
        public string SequenceName { get { return sequenceName; } }
        private Queue<ScenarioElementBeat> current = null;
        private ScenarioElementBeat currentBeatPlaying = null;
        private bool isFinished = false;

        public ScenarioElementBeat CurrentBeatPlaying { get { return currentBeatPlaying; } }
        public int GetCurrentCount { get { return current.Count; } }
        public bool IsFinished { get { return isFinished; } set { isFinished = value; } }

        public ScenarioElementSequence(string sequenceN)
        {
            this.sequenceName = sequenceN;
            InitiaizeSequence();
        }

        private void InitiaizeSequence()
        {
            if (current == null)
            {
                current = new Queue<ScenarioElementBeat>();
            }
        }

        public void AddBeatToCurrent(ScenarioElementBeat beat)
        {
            current.Enqueue(beat);
        }

        public void PlaySequenceElement()
        {
            if(!isFinished)
                currentBeatPlaying = current.Dequeue();
        }
    }
}