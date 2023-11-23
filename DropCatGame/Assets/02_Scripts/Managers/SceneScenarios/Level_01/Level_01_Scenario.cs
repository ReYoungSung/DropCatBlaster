using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using LinearScenarioStructure;

public class Level_01_Scenario: MonoBehaviour
{
    private ScenarioElementChapter chapterLevel_01 = null;
    private LevelStartUpUtility levelStartUpUtility = null;
    private _PlayerInputActions PlayerInputActions = null;
    private ObjectSpawnManager objectSpawnManager = null;
    private GameOver gameOverChapter = null;

    private void Awake()
    {
        chapterLevel_01 = this.GetComponent<ScenarioElementChapter>();
        levelStartUpUtility = this.GetComponent<LevelStartUpUtility>();
        PlayerInputActions = GameObject.Find("[InputManager]").GetComponent<InputManager>().PlayerInputActions;

        objectSpawnManager = GameObject.Find("[ObjectSpawnManager]").GetComponent<ObjectSpawnManager>();
        gameOverChapter = GameObject.Find("[SceneEventManager]").GetComponent<GameOver>();
    }

    public void EnablePlayerInputAction()
    {
        PlayerInputActions.Enable();
    }

    public void DisablePlayerInputAction()
    {
        PlayerInputActions.Disable();
    }

    private void Start()
    {
        GameOverEventManager.current.onGameOverTriggerEnter += gameOverChapter.GetGameOverChapter.StartChapter;
        chapterLevel_01.LevelLoadRoutine(LoadSequence_Level01);
    
    }

    private void LoadSequence_Level01()
    {
        ScenarioElementSequence levelSequence = new ScenarioElementSequence("Level Sequence");
        ScenarioElementBeat logoDisplay = new ScenarioElementBeat(new TimerBasedReaction(0.3f));
        logoDisplay.AddEventsToResponse(
                delegate
                {
                    StartCoroutine(levelStartUpUtility.DisplayLogo(0.9f));
                }
                );
        levelSequence.AddBeatToCurrent(logoDisplay);

        ScenarioElementBeat missionMessageDisplay = new ScenarioElementBeat(new TimerBasedReaction(1.5f));
        missionMessageDisplay.AddEventsToResponse(
                delegate
                {
                    levelStartUpUtility.PopUpObjectiveMark(objectSpawnManager.GetModeName);
                }
                );
        levelSequence.AddBeatToCurrent(missionMessageDisplay);

        ScenarioElementBeat enemySpawnTrigger = new ScenarioElementBeat(new TimerBasedReaction(3f));
        missionMessageDisplay.AddEventsToResponse(
                delegate
                {
                    objectSpawnManager.SpawningTriggerEnter();
                    levelSequence.IsFinished = true;
                }
                );
        levelSequence.AddBeatToCurrent(enemySpawnTrigger);
        chapterLevel_01.AddSequenceToCurrent(levelSequence);
    }

    /*
    private void Load_PopUpDialogue(int loop)
    {
        for (int i = 0; i < loop; i++)
        {
            PopUpMessage beat_01 = new PopUpMessage();
            string dialogue = tutorialDialogue.Dequeue();
            if (i < 1)
            {
                beat_01.AddEventsToBeatEnter(popUpMessageBehaviour.PopsUp);
                beat_01.AddEventsToBeat(audioManager.PlaySFX, "DialoguePopsUp");
            }
            beat_01.AddEventsToBeat(popUpMessageBehaviour.SwitchContents, dialogue);
            sequence_01.AddBeatToCurrent(beat_01);
        }
    }
    
    private void Load_Tutorial(int loop)
    {
        for (int i = 0; i < loop; i++)
        {
            PopUpMessage beat_01 = new PopUpMessage();
            IntervalBreak intervalBreak = new IntervalBreak();
            string dialogue = tutorialDialogue.Dequeue();
            if (i == 4)
            {
                beat_01.AddEventsToBeat(delegate { catHousesForTutorial[2] = objectSpawnManager.SpawnObject(1, 6); });
                beat_01.AddEventsToBeat(SetCameraTargetToCatHouse);
            }
            else if(i == 9)
            {
                beat_01.AddEventsToBeat(SetCameraTargetToPlayer);
            }
            else if(i == 10)
            {
                beat_01.AddEventsToBeat(delegate { catHousesForTutorial[0] = objectSpawnManager.SpawnObject(1, 1); });
            }
            else if (i == 11)
            {
                beat_01.AddEventsToBeat(delegate { catHousesForTutorial[1] = objectSpawnManager.SpawnObject(1, new Vector2(39f, 59f)); });
                beat_01.AddEventsToBeat(delegate { DisableFalling(ref catHousesForTutorial[1]); });
                beat_01.AddEventsToBeat(delegate { timer_2ND.StartTicking(intervalBreakTime); });
            }
            beat_01.AddEventsToBeat(popUpMessageBehaviour.SwitchContents, dialogue);
            sequence_01.AddBeatToCurrent(beat_01);
        }
    }
    */
}
