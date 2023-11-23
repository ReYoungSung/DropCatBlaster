using System;
using UnityEngine;

public class LevelCompleteEventManager : MonoBehaviour
{
    public static LevelCompleteEventManager current;

    private void Awake()
    {
        current = this;
    }
    public event Action onLevelCompleteTriggerEnter;

    public void levelCompleteTriggerEnter()
    {
        if (onLevelCompleteTriggerEnter != null)
        {
            onLevelCompleteTriggerEnter();
        }
    }
}
