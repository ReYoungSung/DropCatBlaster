using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LinearScenarioStructure
{
    public class ScenarioElementBeat
    {
        // Smallest element that composes one scenario event 
        // This can be a page of dialogue, a cutscene, etc.
        private UnityEvent beatTriggerAction = null;
        private IScenarioElementPlayerReaction playerReaction = null;
        private UnityEvent beatTriggerResponse = null;

        private bool isFinished = false;
        public bool IsFinished { get { return isFinished; } }

        public ScenarioElementBeat(IScenarioElementPlayerReaction playerReaction)
        {
            InitializeBeat(playerReaction);
        }

        public virtual void InitializeBeat(IScenarioElementPlayerReaction assignedReaction)
        {
            if (beatTriggerAction == null)
            {
                beatTriggerAction = new UnityEvent();
            }
            if (playerReaction == null)
            {
                playerReaction = assignedReaction;
            }
            if (beatTriggerResponse == null)
            {
                beatTriggerResponse = new UnityEvent();
            }
        }

        public void AddEventsToAction(UnityAction eventMethod)
        {
            beatTriggerAction.AddListener(eventMethod);
        }

        public void AddEventsToResponse(UnityAction eventMethod)
        {
            beatTriggerResponse.AddListener(eventMethod);
        }

        public IEnumerator PlayBeatRoutine()
        {
            while(!TransitBeat())
            {
                yield return null;
                if (TransitBeat())
                {
                    if(!isFinished)
                    {
                        beatTriggerResponse.Invoke();
                        MarkBeatAsFinished();
                        yield break;
                    }
                }
            }
        }

        public void MarkBeatAsFinished()
        {
            isFinished = true;
        }

        public virtual bool TransitBeat()
        {
            return playerReaction.Reaction();
        }
    }
}