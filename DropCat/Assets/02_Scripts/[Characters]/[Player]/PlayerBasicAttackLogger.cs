using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttackLogger : MonoBehaviour
{
    private float comboTimer = 2f;
    private float comboTimerCache = 0;
    private bool timerIsOn = false;
    private int basicAttackComboCount = 0;
    private int comboIncrement = 0;
    private int maxComboCount = 0;
    private int playerScore = 0;
    private int fullPlayerScore = 0;
    private int currentAdditionalComboScore = 5;
    private ObjectSpawnManager objectSpawnManager = null;
    private TimeAttack timeAttack;

    public int BasicAttackComboCount { get { return basicAttackComboCount; }  }
    public float FullPlayerScore { get { return fullPlayerScore; }  }
    public float PlayerScore { get { return playerScore; } }
    public int MaxComboCount { get { return maxComboCount; } }
    public int CurrentAdditionalComboScore { get { return currentAdditionalComboScore; } }

    private void Awake()
    {
        objectSpawnManager = GameObject.Find("[ObjectSpawnManager]").GetComponent<ObjectSpawnManager>();
        timeAttack = GameObject.Find("[SceneEventManager]").GetComponent<TimeAttack>();
        CatTowerBasicScore();
    }
    
    public void decreasePlayerScore()
    {
        if(objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.DjClimbing)
        {
            playerScore = playerScore*(int)timeAttack.SelectCountdown/(int)timeAttack.TimeLimit;
        }
    }

    public void CatTowerBasicScore()
    {
        if(objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.DjClimbing)
        {
            playerScore = (int)timeAttack.TimeLimit * 10;
            fullPlayerScore = playerScore;
        }
    }

    public void IncreasePlayerScore(int scorePerPunch)
    {
        if(objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.CatHouseFalling)
        {
            playerScore += scorePerPunch;
            playerScore += currentAdditionalComboScore;
        }
    }

    public void IncreaseComboCount()
    {
        comboIncrement += 1;
    }

    public void ResetComboCount()
    {
        basicAttackComboCount = 0;
    }

    public void StartComboTimer()
    {
        if(1 < comboIncrement)
        {
            if (!timerIsOn)
            {
                timerIsOn = true;
            }
        }
    }

    public bool ShouldDisplayUIFeedback()
    {
        return timerIsOn;
    }

    private void Update()
    {
        defineAdditionalComboScore();
        
        if (timerIsOn)
        {
            RunComboTimer();
        }
        else
        {
            ResetComboTimer();
        }
    }

    private void RunComboTimer()
    {
        if (basicAttackComboCount == comboIncrement)
        {
            if(basicAttackComboCount != 0 && comboIncrement != 0)
            {
                comboTimerCache -= Time.deltaTime;
            }
            if (comboTimerCache <= 0)
            {
                comboTimerCache = 0;
                timerIsOn = false;
            }
        }
        else if (basicAttackComboCount < comboIncrement)
        {
            comboTimerCache = comboTimer;
            basicAttackComboCount = comboIncrement;
            UpdateMaxComboCount();
        }
        else
        {
            return;
        }

    }
    
    private void ResetComboTimer()
    {
        if(!timerIsOn && comboTimerCache != comboTimer)
        {
            comboTimerCache = comboTimer;
            comboIncrement = 0;
            basicAttackComboCount = 0;
        }
    }

    private void UpdateMaxComboCount()
    {
        if(maxComboCount < comboIncrement)
        {
            maxComboCount = comboIncrement;
        }
    }

    private void defineAdditionalComboScore()
    {
        if(5 <= basicAttackComboCount && basicAttackComboCount < 10)
        {
            currentAdditionalComboScore = 5;
        }
        else if(10 <= basicAttackComboCount && basicAttackComboCount < 25)
        {
            currentAdditionalComboScore = 10;
        }
        else if(25 <= basicAttackComboCount && basicAttackComboCount < 50)
        {
            currentAdditionalComboScore = 20;
        }
        else if(50 <= basicAttackComboCount && basicAttackComboCount < 100)
        {
            currentAdditionalComboScore = 40;
        }
        else if(100 <= basicAttackComboCount)
        {
            currentAdditionalComboScore = 80;
        }
        else
        {
            currentAdditionalComboScore = 0;
        }
    }
}