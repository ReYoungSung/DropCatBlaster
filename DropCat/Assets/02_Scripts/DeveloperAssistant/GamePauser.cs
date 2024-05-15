using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePauser : MonoBehaviour
{
    [SerializeField] private bool gameIsPaused = false;
    public bool GameIsPaused { get { return gameIsPaused; } set { gameIsPaused = value; } }

    public event Action OnGamePaused;

    public void PauseOn(bool pause)
    {
        if (pause)
        {
            if(Time.timeScale > 0)
            {
                gameIsPaused = true;
                Time.timeScale = 0f;
            }
        }
        else
        {
            if(Time.timeScale <= 0)
            {
                gameIsPaused = false;
                Time.timeScale = 1f;
            }
        }
    }
}
