using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using CharacterBehaviour;

public class LevelComplete : MonoBehaviour
{
    private LevelCompleteEventManager levelCompleteEventManager = null;

    private Transform playerObj = null;
    private PlayerBasicAttackLogger playerBasicAttackLogger = null;
    private bool once = true;
    [SerializeField] private GameObject levelCompleteMenuUI = null;

    private Chapter chapterLevelComplete = null;
    private ObjectSpawnManager objectSpawnManager = null;

    private EnemyEffects enemyEffectsControl = null;
    private LevelCompleteMenuManager levelCompleteMenuManager = null;

    private bool levelComplete = false;

    private int catHouseCache = 0;
    private Timer timer = null;
    [SerializeField] private GameObject[] completionMark = null;
    public UnityEvent OnCompletion;

    private Coroutine coroutine = null; 
    private Coroutine effectCoroutine = null; 
    private LevelLoader levelLoader = null; 
    [SerializeField] private BaseHouseDurability baseHouseDruability = null;
    public BaseHouseDurability BaseHouseDruability { get { return baseHouseDruability; } }

    [SerializeField] private int motherShipDamaged = 0;

    private GameObject buttonPressAdvice = null;
    [SerializeField] private GameObject buttonPressAdvice_Gamepad = null;
    [SerializeField] private GameObject buttonPressAdvice_Keyboard = null;

    private GameObject gamePlayCanvas;
    private _PlayerInputActions PlayerInputActions = null;

    private CatSpaceshipHatchClosing catSpaceshipHatchClosing;

    private PauseManager pauseManager;
    

    private void Awake()
    {
        gamePlayCanvas = GameObject.Find("[UI] GamePlayCanvas"); 
        enemyEffectsControl = this.GetComponent<EnemyEffects>(); 
        levelCompleteMenuManager = this.GetComponent<LevelCompleteMenuManager>(); 
        objectSpawnManager = GameObject.Find("[ObjectSpawnManager]").GetComponent<ObjectSpawnManager>(); 
        playerObj = GameObject.Find("[Player]").transform; 
        playerBasicAttackLogger = playerObj.GetChild(0).GetComponent<PlayerBasicAttackLogger>(); 
        chapterLevelComplete = new Chapter(); 
        timer = this.GetComponent<Timer>(); 
        levelLoader = GameObject.Find("[LevelLoader]").GetComponent<LevelLoader>(); 

        baseHouseDruability = baseHouseDruability.gameObject.GetComponent<BaseHouseDurability>(); 
        catSpaceshipHatchClosing = GameObject.FindGameObjectWithTag("ENEMY_Spaceship").GetComponent<CatSpaceshipHatchClosing>(); 

        string device = VerifyDeviceManager.VerifyCurrentDevice();
        buttonPressAdvice_Gamepad.SetActive(false);
        buttonPressAdvice_Keyboard.SetActive(false);
        if (buttonPressAdvice == null)
        {
            if (device == "Gamepad")
                buttonPressAdvice = buttonPressAdvice_Gamepad; 
            else if (device == "Keyboard")
                buttonPressAdvice = buttonPressAdvice_Keyboard; 
        }
        PlayerInputActions = GameObject.Find("[InputManager]").GetComponent<InputManager>().PlayerInputActions;

        levelCompleteEventManager = this.GetComponent<LevelCompleteEventManager>();
        pauseManager = this.GetComponent<PauseManager>();

        levelCompleteMenuUI = GameObject.Find("LevelCompleteMenu");
    }

    private void Start()
    {
        RemainListAsInComplete();
        LoadSequence_LevelComplete();
        LevelCompleteEventManager.current.onLevelCompleteTriggerEnter += chapterLevelComplete.StartChapter;
    }

    public void Update()
    {
        if(objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.CatHouseFalling)
        {
            if (objectSpawnManager.LevelFinished)
            {
                if(0 < baseHouseDruability.Durability)
                {   
                    LevelCompleteEventManager.current.levelCompleteTriggerEnter();
                    PlayerInputActions.Player.Disable();
                }
            }
        }
        else if(objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.DjClimbing)
        {
            if (catSpaceshipHatchClosing.HatchIsClosed == true)
            {
                if(0 < baseHouseDruability.Durability)
                {
                    if(once)
                    {
                        playerBasicAttackLogger.decreasePlayerScore();
                        once = false;
                    }
                    LevelCompleteEventManager.current.levelCompleteTriggerEnter();
                    PlayerInputActions.Player.Disable();
                }
            }
        }
    }

    private void LoadSequence_LevelComplete()
    {
        Sequence levelCompleteSeq = new Sequence("LevelComplete");
        for (int i = 0; i < 2; i++)
        {
            Beat beatCache = new Beat();
            if (i == 0)
            {
                beatCache.AddEventsToBeatRepeat(
                    delegate
                    {
                        levelLoader.IncreaseMaxLevelCompleted();
                        PlayerInputActions.UIs.Enable();
                        PlayerInputActions.Player.Disable();
                        timer.StartTicking();
                        coroutine = StartCoroutine(timer.CoroutineTimer(0.7f, levelCompleteSeq.ContinueSequence));
                    }
                );
                beatCache.AddEventsToBeatExit(LevelCompletePausePopUp);
            }
            levelCompleteSeq.AddBeatToCurrent(beatCache);
        }
        chapterLevelComplete.AddSequenceToCurrent(levelCompleteSeq);
    }

    private void LevelCompletePausePopUp()
    {
        if (0 < baseHouseDruability.Durability)
        {
            gamePlayCanvas.SetActive(false);
            levelCompleteMenuManager.InitializeScore(playerBasicAttackLogger.PlayerScore, playerBasicAttackLogger.MaxComboCount);
            levelCompleteMenuManager.LevelCompletePause(levelCompleteMenuUI);
            levelCompleteMenuManager.LevelCompleteMenuRoutine();
            levelLoader.SaveLevel(levelCompleteMenuManager.FinalStarRecord);
        }
    }

    public void LoadNextLevel()
    {
        levelLoader.GetComponent<LevelLoader>().LoadNextScene();
    }

    private void MarkListAsComplete()
    {
        foreach (GameObject mark in completionMark)
            mark.SetActive(true);
        buttonPressAdvice.SetActive(true);
    }

    private void RemainListAsInComplete()
    {
        foreach(GameObject mark in completionMark)
            mark.SetActive(false);
        buttonPressAdvice.SetActive(false);
    }

    private IEnumerator SceneTransition(InputAction tutorialAction)
    {
        Debug.Log("ENTERERD");
        bool transitionCondition = false;
        while (!transitionCondition)
        {
            transitionCondition = tutorialAction.triggered;
            yield return null;
            if (transitionCondition)
            {
                levelLoader.LoadNextScene();
            }
        }
    }
}
