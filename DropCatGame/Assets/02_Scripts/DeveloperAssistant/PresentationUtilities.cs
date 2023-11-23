using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PresentationUtilities : MonoBehaviour
{
    private _PlayerInputActions PlayerInputActions;
    [SerializeField] private GameObject[] developerToolHolders;
    private bool developerModeIsOn;

    [SerializeField] private TMPro.TextMeshProUGUI statusText;
    [SerializeField] private float messageDisplayDuration = 3f;
    private bool isMessageDisplaying = false;

    private Timer devTimer;
    private GamePauser devPauser;
    private EnemySpawner devEnemySpawner;
    private PropEnabler devPropEnabler;

    private void Awake()
    {
        Initialize();
        devTimer = GetComponent<Timer>();
        devPauser = GetComponent<GamePauser>();
        devEnemySpawner = developerToolHolders[0].GetComponent<EnemySpawner>();
        devPropEnabler = developerToolHolders[1].GetComponent<PropEnabler>();
        devTimer.timeUpTriggerEnter += HideDisplayedMessage;
        PlayerInputActions = new _PlayerInputActions();
    }
    
    private void Initialize()
    {
        if (gameObject)
        {
            foreach (GameObject developerTool in developerToolHolders)
            {
                developerTool.SetActive(false);
            }
            developerModeIsOn = false;
        }
    }

    private void Update()
    {
        EnableDeveloperMode();
        if(developerModeIsOn)
        {
            DeveloperPause();
            DeveloperSpawnEnemy();
            DeveloperSwitchProp();
        }
    }

    private void EnableDeveloperMode()
    {
        if(PlayerInputActions.Development.EnableDeveloperMode.triggered)
        {
            if(!developerModeIsOn)
            {
                foreach(GameObject developerTool in developerToolHolders)
                {
                    developerTool.SetActive(true);
                }
                statusText.text = "Developer Mode On";
                developerModeIsOn = true;
            }
            else
            {
                foreach (GameObject developerTool in developerToolHolders)
                {
                    developerTool.SetActive(false);
                }
                statusText.text = "Developer Mode Off";
                developerModeIsOn = false;
            }
            DisplayConsoleInfo(messageDisplayDuration);
        }
    }

    private void DeveloperPause()
    {
        if (PlayerInputActions.Development.PauseGame.triggered)
        {
            if (devPauser.GameIsPaused == false)
            {
                statusText.text = "Game is paused";
                devPauser.PauseOn(true);
            }
            else
            {
                statusText.text = "Game is resumed";
                devPauser.PauseOn(false);
            }
            DisplayConsoleInfo(messageDisplayDuration);
        }
    }

    private void DeveloperSpawnEnemy()
    {
        if (PlayerInputActions.Development.SpawnEnemy.triggered)
        {
            if (devEnemySpawner.SwitchSpawning())
            {
                statusText.text = "[Activated] Enemy Spawning";
            }
            else
            {
                statusText.text = "[Stopped] Enemy Spawning";
            }
                DisplayConsoleInfo(messageDisplayDuration);

        }
    }

    private void DeveloperSwitchProp()
    {
        if (PlayerInputActions.Development.EnableProp.triggered)
        {
            devPropEnabler.EnableTargetObj();
            statusText.text = "Prop Enabled : " + devPropEnabler.TargetObj[0].activeSelf;
            DisplayConsoleInfo(messageDisplayDuration);
        }
    }

    private void DisplayConsoleInfo(float displayTime)
    {
        if (!isMessageDisplaying)
        {
            isMessageDisplaying = true;
            LeanTween.moveLocalX(statusText.gameObject, 600f, 0.5f).setEaseOutCirc().setIgnoreTimeScale(true);
            devTimer.StartTicking();
        }
    }
    private void DisplayConsoleInfo(float displayTime, string info)
    {
        if (!isMessageDisplaying)
        {
            isMessageDisplaying = true;
            statusText.text = info;
            LeanTween.moveLocalX(statusText.gameObject, 600f, 0.5f).setEaseOutCirc().setIgnoreTimeScale(true);
            devTimer.StartTicking();
        }
    }

    private void HideDisplayedMessage()
    {
        if(isMessageDisplaying)
        {
            LeanTween.moveLocalX(statusText.gameObject, 1340f, 0.5f).setEaseOutCirc().setIgnoreTimeScale(true);
            isMessageDisplaying = false;
        }
    }
}
