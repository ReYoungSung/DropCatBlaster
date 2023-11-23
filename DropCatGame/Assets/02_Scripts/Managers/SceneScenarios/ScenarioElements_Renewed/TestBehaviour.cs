using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LinearScenarioStructure
{
    public class InputBasedReaction : IScenarioElementPlayerReaction
    { 
        InputAction inputAction = null;
        public bool Triggered { get { return inputAction.triggered; } }

        public InputBasedReaction(InputAction inputAction)
        {
            this.inputAction = inputAction;
        }

        bool IScenarioElementPlayerReaction.Reaction()
        {
            if (inputAction.triggered)
            {
                return true;
            }
            return false;
        }
    }

    public class TaskBasedReaction : IScenarioElementPlayerReaction
    {
        Func<bool> taskOnDemand = null;
        Action accompaniedAction = null;
        bool actionExecuted = false;
        bool taskExecuted = false;

        public TaskBasedReaction(Func<bool> taskMethod)
        {
            taskOnDemand = taskMethod;
        }

        public TaskBasedReaction(Func<bool> taskMethod, Action accompaniedAction)
        {
            this.accompaniedAction += accompaniedAction;
            taskOnDemand = taskMethod;
        }

        bool IScenarioElementPlayerReaction.Reaction()
        {
            Task();
            return taskExecuted;
        }

        private void Task()
        {
            if (taskOnDemand.Invoke())
            {
                if (!actionExecuted)
                {
                    if (accompaniedAction != null)
                    {
                        accompaniedAction.Invoke();
                        actionExecuted = true;
                    }
                }
                taskExecuted = true;
            }
        }
    }

    public class TimerBasedReaction : IScenarioElementPlayerReaction
    {
        private float timer = 3f;
        private bool timerDone = false;
        private MonoBehaviour monoBehaviour;
        private Coroutine coroutine = null;

        public TimerBasedReaction(float time)
        {
            this.timer = time;
            monoBehaviour = GameObject.FindObjectOfType<MonoBehaviour>();
        }

        private IEnumerator timerRoutine()
        {
            yield return new WaitForSeconds(timer);
            timerDone = true;
        }

        bool IScenarioElementPlayerReaction.Reaction()
        {
            if(coroutine == null)
                coroutine = monoBehaviour.StartCoroutine(timerRoutine());
            return timerDone;
        }
    }

    public class InstantReaction : IScenarioElementPlayerReaction
    {
        bool IScenarioElementPlayerReaction.Reaction()
        {
            return true;
        }
    }
}
