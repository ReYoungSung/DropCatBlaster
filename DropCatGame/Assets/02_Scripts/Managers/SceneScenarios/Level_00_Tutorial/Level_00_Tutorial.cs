using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LinearScenarioStructure;

public class Level_00_Tutorial : MonoBehaviour
{
    private ScenarioElementChapter chapterTutorial = null;
    private const int indexIntroduction = 8;

    private Queue<string> tutorialDialogue = null;
    private DialogueDataReader dialogueDataReader = null;
    private InputManager inputManager = null;

    private PopUpMessageBehaviour popUpMessageBehaviour = null;
    private InputBasedReaction continueDialogueAction = null;

    private GameObject player = null;
    [SerializeField] private GameObject virtualCamera = null;
    [SerializeField] private Transform sky = null;
    private ObjectSpawnManager objectSpawnManager = null;
    private List<GameObject> catHousesForTutorial = new List<GameObject>();
    private LevelStartUpUtility levelStartUpUtility = null;
    private Coroutine coroutine = null;
    private LevelLoader levelLoader = null;

    private bool catHouseDemoDone = false;

    private void Awake()
    {
        chapterTutorial = this.GetComponent<ScenarioElementChapter>();
        tutorialDialogue = new Queue<string>();
        string device = VerifyDeviceManager.VerifyCurrentDevice();
        dialogueDataReader = new DialogueDataReader("level_00_tutorial");
        dialogueDataReader.EnqueueDialogues("Tutorial", ref tutorialDialogue);
        inputManager = GameObject.Find("[InputManager]").GetComponent<InputManager>();
        GameObject popUpMessage = GameObject.Find("[EventDrivenCanvas]").transform.GetChild(1).gameObject;
        popUpMessageBehaviour = popUpMessage.GetComponent<PopUpMessageBehaviour>();
        continueDialogueAction = new InputBasedReaction(inputManager.PlayerInputActions.Player.TutorialContinueDialogue);
        player = GameObject.Find("[Player]");
        objectSpawnManager = GameObject.Find("[ObjectSpawnManager]").GetComponent<ObjectSpawnManager>();
        objectSpawnManager.CatHouseCount = 100;
        levelStartUpUtility = this.GetComponent<LevelStartUpUtility>();
        levelLoader = GameObject.Find("[LevelLoader]").GetComponent<LevelLoader>();
    }

    void Start()
    {
        chapterTutorial.LevelLoadRoutine(Load_Introduction);
    }

    private void Load_Introduction()
    {
        ScenarioElementSequence introductionPopUpMsg = new ScenarioElementSequence("Introduction PopUp Msg");

        ScenarioElementBeat delayMessagePopUp = new ScenarioElementBeat(new TimerBasedReaction(2.0f));
        delayMessagePopUp.AddEventsToResponse(
                delegate
                {
                    popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
                    popUpMessageBehaviour.PopsUpMessage();
                }
                );
        introductionPopUpMsg.AddBeatToCurrent(delayMessagePopUp);

        for (int i = 0; i < indexIntroduction; i++)
        {
            ScenarioElementBeat introductionCache = new ScenarioElementBeat(continueDialogueAction);
            introductionCache.AddEventsToResponse(
                delegate
                {
                    popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
                }
                );
            if(i == indexIntroduction - 1)
            {
                introductionCache.AddEventsToResponse(
                delegate
                {
                    popUpMessageBehaviour.ToggleButtonPressAdvice(false);
                    popUpMessageBehaviour.PullsBackButtonPressAdvice();
                }
                );
            }
            introductionPopUpMsg.AddBeatToCurrent(introductionCache);
        }

        // 이동 튜토리얼 
        ScenarioElementBeat moveTutorialCache_01 = new ScenarioElementBeat(new TaskBasedReaction(JoystickIsMoved, PassCheckMarkRoutine));
        moveTutorialCache_01.AddEventsToResponse(
                delegate
                {
                    popUpMessageBehaviour.ToggleButtonPressAdvice(true);
                    popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
                    popUpMessageBehaviour.PopUpButtonPressAdvice();
                }
                );
        introductionPopUpMsg.AddBeatToCurrent(moveTutorialCache_01);

        ScenarioElementBeat moveTutorialCache_02 = new ScenarioElementBeat(continueDialogueAction);
        moveTutorialCache_02.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
                popUpMessageBehaviour.PullsBackButtonPressAdvice();
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(moveTutorialCache_02);

        // 점프 튜토리얼
        ScenarioElementBeat jumpTutorialCache_01 = new ScenarioElementBeat(new TaskBasedReaction(JumpButtonIsPressed, PassCheckMarkRoutine));
        jumpTutorialCache_01.AddEventsToResponse(
                delegate
                {
                    popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
                    popUpMessageBehaviour.PopUpButtonPressAdvice();
                }
                );
        introductionPopUpMsg.AddBeatToCurrent(jumpTutorialCache_01);

        ScenarioElementBeat moveTutorialCache_03 = new ScenarioElementBeat(continueDialogueAction);
        moveTutorialCache_03.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
                popUpMessageBehaviour.PopUpButtonPressAdvice();
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(moveTutorialCache_03);

        ScenarioElementBeat attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
                popUpMessageBehaviour.PopUpButtonPressAdvice();
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        // 고양이 집 소개
        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
                popUpMessageBehaviour.PullsBackButtonPressAdvice();
                objectSpawnManager.SpawnSingleRow(ref catHousesForTutorial);
                SetCameraTargetToCatHouse();
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(new TaskBasedReaction(AllCatHouseDestroyed, popUpMessageBehaviour.PopUpButtonPressAdvice));
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
                if(0 < catHousesForTutorial.Count)
                {
                    popUpMessageBehaviour.PullsBackButtonPressAdvice();
                }
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
                popUpMessageBehaviour.ToggleButtonPressAdvice(false);
                popUpMessageBehaviour.PullsBackButtonPressAdvice();
                catHousesForTutorial.Add(objectSpawnManager.SpawnObject(0, new Vector2(194f, -90f)));
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(new TaskBasedReaction(AllCatHouseDestroyed, PassCheckMarkRoutine));
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
                catHousesForTutorial.Add(objectSpawnManager.SpawnObject(0, new Vector2(-68f, -90f)));
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(new TaskBasedReaction(AllCatHouseDestroyed, PassCheckMarkRoutine));
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
                catHousesForTutorial.Add(objectSpawnManager.SpawnObject(0, new Vector2(194f, -90f)));
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(new TaskBasedReaction(AllCatHouseDestroyed, PassCheckMarkRoutine));
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.ToggleButtonPressAdvice(true);
                popUpMessageBehaviour.PopUpButtonPressAdvice();
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.PullsBackMessage();
                popUpMessageBehaviour.PullsBackButtonPressAdvice();
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(new TimerBasedReaction(1.5f));
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                coroutine = StartCoroutine(levelStartUpUtility.DisplayLogo(0.9f));
                objectSpawnManager.onSpawningTriggerEnter.Invoke();
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(new TimerBasedReaction(1.5f));
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                levelStartUpUtility.PopUpObjectiveMark();
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(new TaskBasedReaction(AllCatHouseDestroyed, PassCheckMarkRoutine));
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.PopsUpMessage();
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                popUpMessageBehaviour.SwitchContents(tutorialDialogue.Dequeue());
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);

        attackTutorialCache = new ScenarioElementBeat(continueDialogueAction);
        attackTutorialCache.AddEventsToResponse(
            delegate
            {
                introductionPopUpMsg.IsFinished = true;
                levelLoader.LoadNextScene();
            }
            );
        introductionPopUpMsg.AddBeatToCurrent(attackTutorialCache);
        chapterTutorial.AddSequenceToCurrent(introductionPopUpMsg);
    }

    private void Update()
    {
        if (0 < catHousesForTutorial.Count)
        {
            if (catHousesForTutorial[0] == null)
            {
                catHousesForTutorial.Clear();
            }
        }
        if(4 == objectSpawnManager.SpawnedCatHouse)
        {
            objectSpawnManager.onSpawningTriggerExit.Invoke();
        }
    }

    private bool JoystickIsMoved()
    {
        return inputManager.MoveJoystick.Horizontal != 0f;
    }

    private bool JumpButtonIsPressed()
    {
        return inputManager.PlayerInputActions.Player.PlayerJump_Mobile.triggered;
    }

    private void PassCheckMarkRoutine()
    {
        levelStartUpUtility.DisplayCheckMark();
    }

    private void SetCameraTargetToCatHouse()
    {
        sky.GetComponent<SkyCameraTracker>().Target = virtualCamera.transform;
        virtualCamera.GetComponent<CameraConstraint>().enabled = false;
        catHousesForTutorial[0].transform.position = new Vector2(65, catHousesForTutorial[0].transform.position.y);
        virtualCamera.GetComponent<CameraFocusOperation>().TargetObject = catHousesForTutorial[0];
        virtualCamera.GetComponent<CameraFocusOperation>().FocusCameraToObject(catHousesForTutorial[0], 1f);

        player.transform.position = new Vector2(65, 52);
    }

    private void SetCameraTargetToPlayer()
    {
        sky.GetComponent<SkyCameraTracker>().Target = player.transform;
        virtualCamera.GetComponent<CameraFocusOperation>().TargetObject = player;
        virtualCamera.GetComponent<CameraConstraint>().enabled = true;
    }

    private bool AllCatHouseDestroyed()
    {
        if(catHousesForTutorial.Count <= 0)
        {
            foreach(GameObject catHouse in GameObject.FindGameObjectsWithTag("ENEMY_CatHouse"))
            {
                catHousesForTutorial.Add(catHouse);
            }
        }
        if(catHousesForTutorial.Count <= 0)
        {
            return true;
        }
        return false;
    }
}
