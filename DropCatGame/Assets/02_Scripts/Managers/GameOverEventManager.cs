using System;
using UnityEngine;

public class GameOverEventManager : MonoBehaviour
{
    public static GameOverEventManager current;

    private void Awake()
    {
        current = this;
    }
    public event Action onGameOverTriggerEnter;

    public void GameOverTriggerEnter()
    {
        if(onGameOverTriggerEnter != null)
        {
            onGameOverTriggerEnter();
        }
    }
}
