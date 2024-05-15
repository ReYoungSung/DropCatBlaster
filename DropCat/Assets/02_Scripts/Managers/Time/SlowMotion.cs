using UnityEngine;

public class SlowMotion: MonoBehaviour
{
    public static SlowMotion instance = null;
    private static float slowDownLength = 0f;

    private void Awake()
    {
        instance = this;
    }

    public void ActivateSlowMotion(float slowDownFactor, float slowDownLengthInSeconds)
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        slowDownLength = slowDownLengthInSeconds;
    }

    public void RestoreGameTime(bool gameIsPaused)
    {
        if (!gameIsPaused)
        {
            Time.timeScale = 1f;
        }
    }
}
