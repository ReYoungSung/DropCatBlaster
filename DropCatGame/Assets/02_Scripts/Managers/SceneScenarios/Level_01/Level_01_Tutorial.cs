/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CharacterManager;
using CharacterBehaviour.Enemy.CatHouse;

public class Level_01_Tutorial : MonoBehaviour
{
    private Chapter chapterLevel_01 = null;
    private Queue<string> tutorialDialogue = null;
    private DialogueDataReader dialogueDataReader = null;
    private _PlayerInputActions PlayerInputActions = null;

    [SerializeField] private GameObject virtualCamera = null;
    private PopUpMessageBehaviour popUpMessageBehaviour = null;
    private AudioManager audioManager = null;
    private Timer timer = null;
    private Coroutine tutorialCoroutine = null;

    [SerializeField] private GameObject popUpMessage = null;
    private GameObject[] catHousesForTutorial = new GameObject[3];
    [SerializeField] private Transform player = null;
    [SerializeField] private Transform sky = null;
    [SerializeField] private GameObject houseUI = null;

    private ObjectSpawnManager objectSpawnManager = null;
    private GameOver gameOverChapter = null;

    private bool moveTutorialComplete;
    private bool jumpTutorialComplete;
    private bool teleportTutorialComplete;

    [SerializeField] private GameObject sceneManager = null;
    private LevelCompleteEventManager levelCompleteEventManager = null;
    [SerializeField] private LevelLoader levelLoader = null;

    private PlayerManager movementManager = null;
    private int tutorialIndex = 0;

    private void Awake()
    {
        chapterLevel_01 = new Chapter();
        tutorialDialogue = new Queue<string>();
        PlayerInputActions = GameObject.Find("[InputManager]").GetComponent<InputManager>().PlayerInputActions;
        popUpMessageBehaviour = popUpMessage.GetComponent<PopUpMessageBehaviour>();
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
        objectSpawnManager = GameObject.Find("[ObjectSpawnManager]").GetComponent<ObjectSpawnManager>();
        timer = this.GetComponent<Timer>();
        gameOverChapter = this.GetComponent<GameOver>();
        houseUI.SetActive(false);
        string device = VerifyDeviceManager.VerifyCurrentDevice();
        dialogueDataReader = new DialogueDataReader("Tutorial_" + device);
        dialogueDataReader.EnqueueDialogues("Tutorial", ref tutorialDialogue);
        movementManager = GameObject.Find("[GameManager]").GetComponent<PlayerManager>();
    }

    private void Start()
    {
        audioManager.PlayBGM("MainMenu");
        LoadSequence_Tutorial(27);
        PlayerInputActions.Player.TutorialContinueDialogue.performed += ctx => chapterLevel_01.CurrentSequencePlaying.ContinueSequence();
        PlayerInputActions.Player.TutorialContinueDialogue.performed += ctx => chapterLevel_01.CurrentSequencePlaying.ContinueSequence();
        GameOverEventManager.current.onGameOverTriggerEnter += gameOverChapter.GetGameOverChapter.StartChapter;
        levelCompleteEventManager = sceneManager.GetComponent<LevelCompleteEventManager>();
    }

    private void Update()
    {
        Debug.Log(chapterLevel_01.GetCurrentCount);
        if (chapterLevel_01.CurrentStatus == Chapter.ChapterStatus.isLoading)
        {
            chapterLevel_01.StartChapter();
        }
        else if(chapterLevel_01.CurrentStatus == Chapter.ChapterStatus.isPlaying)
        {
            chapterLevel_01.ContinueChapter();
        }

        moveTutorialComplete = Vector2.zero != PlayerInputActions.Player.PlayerMove.ReadValue<Vector2>() ? true : false;
        jumpTutorialComplete = PlayerInputActions.Player.PlayerJump.triggered;
        teleportTutorialComplete = PlayerInputActions.Player.PlayerTeleport.triggered;

        if(Index_MoveTutorial(tutorialIndex) || Index_JumpTutorial(tutorialIndex) 
            || Index_Teleport(tutorialIndex) || (17 <= tutorialIndex && tutorialIndex < 20))
        {
            DisableContinueButton();
        }
        else
        {
            EnableContinueButton();
        }
    }

    private void LoadSequence_Tutorial(int loop)
    {
        Sequence sequence_01 = new Sequence("Tutorial");
        for (int i = 0; i < loop; i++)
        {
            Beat beat_01 = new Beat();
            string dialogue = tutorialDialogue.Dequeue();
            if (StartTutorial(i))
            {
                beat_01.AddEventsToBeatEnter(
                    delegate
                    {
                        popUpMessageBehaviour.SwitchContents(dialogue);
                        timer.StartTicking();
                    }
                    );
                beat_01.AddEventsToBeatRepeat(
                    delegate
                    {
                        timer.TriggerTime = 2f;
                        tutorialCoroutine = StartCoroutine(timer.CoroutineTimer(beat_01));
                    }
                    );
                beat_01.AddEventsToBeatExit(
                    delegate
                    {
                        audioManager.PlaySFX("DialoguePopsUp");
                        popUpMessageBehaviour.PopsUpMessage();
                    }
                    );
            }
            else if (BasicTutorial(i))
            {
                beat_01.AddEventsToBeatEnter(popUpMessageBehaviour.PullsBackButtonPressAdvice);
                beat_01.AddEventsToBeatRepeat(
                    delegate
                    {
                        popUpMessageBehaviour.ToggleButtonPressAdvice(false);
                        popUpMessageBehaviour.SwitchContents(dialogue);
                    }
                    );
                if (Index_MoveTutorial(i))
                {
                    beat_01.AddEventsToBeatRepeat(
                        delegate
                        {
                            tutorialCoroutine = StartCoroutine(TutorialTransition(PlayerInputActions.Player.PlayerMove));
                        }
                        );
                }
                if (Index_JumpTutorial(i))
                {
                    beat_01.AddEventsToBeatRepeat(
                        delegate
                        {
                            tutorialCoroutine = StartCoroutine(TutorialTransition(PlayerInputActions.Player.PlayerJump));
                        }
                        );
                }
                if (Index_Teleport(i))
                {
                    beat_01.AddEventsToBeatRepeat(
                        delegate
                        {
                            tutorialCoroutine = StartCoroutine(TutorialTransition(PlayerInputActions.Player.PlayerTeleport));
                        }
                        );
                    beat_01.AddEventsToBeatExit(
                        delegate
                        {
                            PlayerInputActions.Player.TutorialContinueDialogue.Enable();
                        }
                        );
                }
            }
            else if (PunchEnemyTutorial(i))
            {
                if (i == 12)
                {
                    beat_01.AddEventsToBeatEnter(
                    delegate
                    {
                        popUpMessageBehaviour.SwitchContents(dialogue);
                        catHousesForTutorial[2] = objectSpawnManager.SpawnObject(1, 6);
                        ++objectSpawnManager.CatHouseCount;
                        SetCameraTargetToCatHouse();
                        player.transform.position = new Vector2(-70f, -130f);
                        movementManager.enabled = false;
                    }
                    );
                }
                else if (i == 17)
                {
                    beat_01.AddEventsToBeatEnter(
                        delegate
                        {
                            SetCameraTargetToPlayer();
                        }
                    );

                    beat_01.AddEventsToBeatRepeat(
                        delegate
                        {
                            tutorialCoroutine = StartCoroutine(DestructionTransition(beat_01, 2));
                        }
                        );
                    beat_01.AddEventsToBeatExit(
                        delegate
                        {
                            PlayerInputActions.Player.TutorialContinueDialogue.Enable();
                        }
                        );
                }
                else if (i == 18)
                {
                    beat_01.AddEventsToBeatEnter(
                    delegate
                    {
                        catHousesForTutorial[0] = objectSpawnManager.SpawnObject(1, 1);
                        ++objectSpawnManager.CatHouseCount;
                    }
                    );
                    beat_01.AddEventsToBeatRepeat(
                        delegate
                        {
                            tutorialCoroutine = StartCoroutine(DestructionTransition(beat_01, 0));
                        }
                        );
                    beat_01.AddEventsToBeatExit(
                        delegate
                        {
                            PlayerInputActions.Player.TutorialContinueDialogue.Enable();
                        }
                        );
                }
                else if (i == 19)
                {
                    Coroutine destCoroutine = null;
                    beat_01.AddEventsToBeatEnter(
                    delegate
                    {
                        popUpMessageBehaviour.PopsUpMessage(-700f, -395f);
                        catHousesForTutorial[1] = objectSpawnManager.SpawnObject(1, new Vector2(39f, 59f));
                        ++objectSpawnManager.CatHouseCount;
                        DisableFalling(catHousesForTutorial[1]);
                    }
                    );

                    beat_01.AddEventsToBeatRepeat(
                        delegate
                        {
                            timer.StartTicking();
                            destCoroutine = StartCoroutine(DestructionTransition(beat_01, 1));
                        }
                        );
                    beat_01.AddEventsToBeatExit(
                        delegate
                        {
                            PlayerInputActions.Player.TutorialContinueDialogue.Enable();
                            popUpMessageBehaviour.PullsBackMessage(-700f);
                        }
                        );
                }
                beat_01.AddEventsToBeatRepeat(
                    delegate
                    {
                        popUpMessageBehaviour.SwitchContents(dialogue);
                    }
                    );
            }
            else if (20 <= i && i < 27)
            {
                if (i == 20)
                {
                    beat_01.AddEventsToBeatRepeat(
                        delegate
                        {
                            audioManager.PlaySFX("DialoguePopsUp");
                            popUpMessageBehaviour.ToggleButtonPressAdvice(true);
                            popUpMessageBehaviour.PopsUpMessage();
                        }
                        );
                }
                else if (i == 26)
                {
                    beat_01.AddEventsToBeatExit(
                        delegate
                        {
                            popUpMessageBehaviour.PullsBackMessage(800f);
                            audioManager.StopBGM();
                            levelLoader.gameObject.GetComponent<LevelLoader>().LoadNextScene();
                        }
                        );
                }
                beat_01.AddEventsToBeatEnter(
                        delegate
                        {
                            popUpMessageBehaviour.SwitchContents(dialogue);
                        }
                        );
            }
            Beat additiveIndex = new Beat();
            additiveIndex.AddEventsToBeatExit(() => ++tutorialIndex);
            sequence_01.AddBeatToCurrent(beat_01);
            sequence_01.AddBeatToCurrent(additiveIndex);
        }
        chapterLevel_01.AddSequenceToCurrent(sequence_01);
    }

    private bool StartTutorial(int i)
    {
        return i == 0;
    }

    private bool BasicTutorial(int i)
    {
        return 1 <= i && i < 12;
    }

    private bool PunchEnemyTutorial(int i)
    {
        return 12 <= i && i < 20;
    }

    private IEnumerator WaitForTriggerActivation(InputAction playerInput)
    {
        bool transitionCondition = false;
        while (!transitionCondition)
        {
            transitionCondition = playerInput.triggered;
            yield return null;
            PopUpCheckMark();
            yield return new WaitForSeconds(0.7f);
            DragDownCheckMark();
            if (transitionCondition)
            {
                levelLoader.gameObject.GetComponent<LevelLoader>().LoadNextScene();
            }
        }
    }

    private IEnumerator DestructionTransition(Beat beat, int index)
    {
        PlayerInputActions.Player.Disable();
        PlayerInputActions.Player.TutorialContinueDialogue.Disable();
        while (!CatHouseDestroyed(index))
        {
            yield return null;
            if (CatHouseDestroyed(index))
            {
                PopUpCheckMark();
                yield return new WaitForSeconds(0.7f);
                DragDownCheckMark();
                chapterLevel_01.CurrentSequencePlaying.ContinueSequence();
                chapterLevel_01.CurrentSequencePlaying.ContinueSequence();
            }
        }
    }

    private IEnumerator TutorialTransition(InputAction tutorialAction)
    {
        bool transitionCondition = false;
        while (!transitionCondition)
        {
            if (PlayerInputActions.Player.enabled)
            {
                PlayerInputActions.Player.TutorialContinueDialogue.Disable();
            }
            else
            {
                PlayerInputActions.Player.TutorialContinueDialogue.Enable();
            }
            transitionCondition = tutorialAction.triggered;
            yield return null;
            if (transitionCondition)
            {
                PopUpCheckMark();
                yield return new WaitForSeconds(0.7f);
                DragDownCheckMark();
                chapterLevel_01.CurrentSequencePlaying.ContinueSequence();
                chapterLevel_01.CurrentSequencePlaying.ContinueSequence();
            }
        }
    }

    private bool CatHouseDestroyed(int index)
    {
        return catHousesForTutorial[index] == null;
    }

    private bool Index_MoveTutorial(int i)
    {
        return i == 8;
    }

    private bool Index_JumpTutorial(int i)
    {
        return i == 9;
    }

    private bool Index_Teleport(int i)
    {
        return i == 10;
    }

    private void SetCameraTargetToCatHouse()
    {
        sky.GetComponent<SkyCameraTracker>().Target = catHousesForTutorial[2].transform;
        virtualCamera.GetComponent<CameraConstraint>().enabled = false;
        virtualCamera.GetComponent<CameraFocusOperation>().TargetObject = catHousesForTutorial[2];
        virtualCamera.GetComponent<CameraFocusOperation>().FocusCameraToObject(catHousesForTutorial[2], 1f);
    }

    private void SetCameraTargetToPlayer()
    {
        sky.GetComponent<SkyCameraTracker>().Target = player.transform;
        virtualCamera.GetComponent<CameraFocusOperation>().TargetObject = player.gameObject;
        virtualCamera.GetComponent<CameraConstraint>().enabled = true;
    }

    private void PopUpCheckMark()
    {
        LeanTween.init();
        checkMark_Tutorial.GetComponent<RectTransform>().LeanSize(checkMarkSize, tweeningTime).setEaseOutCirc().setIgnoreTimeScale(true);
    }

    private void DragDownCheckMark()
    {
        LeanTween.init();
        checkMark_Tutorial.GetComponent<RectTransform>().LeanSize(Vector2.zero, tweeningTime).setEaseOutBounce().setIgnoreTimeScale(true);
    }

    private void DisableFalling(GameObject catHouse)
    {
        catHouse.GetComponent<TutorialCatHouseBehaviour>().IsFalling = false;
        catHouse.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void OnEnable()
    {
        PlayerInputActions.Enable();
    }

    private void EnableContinueButton()
    {
        if (!PlayerInputActions.Player.TutorialContinueDialogue.enabled)
        {
            PlayerInputActions.Player.TutorialContinueDialogue.Enable();
        }
    }

    private void DisableContinueButton()
    {
        if (PlayerInputActions.Player.TutorialContinueDialogue.enabled)
        {
            PlayerInputActions.Player.TutorialContinueDialogue.Disable();
        }
    }
}
*/