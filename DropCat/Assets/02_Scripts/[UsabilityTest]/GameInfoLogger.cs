using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoLogger : MonoBehaviour
{
    public static int maxTrial = 3;
    public static int currentTrial = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public static void InitializeTrial()
    {
        currentTrial = 0;
    }

    public static void IncreaseTrial()
    {
        ++currentTrial;
    }

    public static bool TrialExhausted()
    {
        return maxTrial == currentTrial;
    }
}
