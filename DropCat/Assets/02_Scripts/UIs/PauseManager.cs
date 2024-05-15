using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using CharacterBehaviour.Move;

public class PauseManager : MonoBehaviour
{
    private GameObject pauseMenuUI;
    private GameObject pauseMenu;
    private RectTransform pauseMenuRectTransform;
    public static bool gameIsPaused = false;
    public static bool ComposeNotPaused = false;

    private InputManager inputManager = null;
    private AudioManager audiomanager;
    private PlayerMovement playerMovement;

    private Vector2 originalPosition = new Vector2(79.35f, -1885f);
    private Vector2 scrollUpPosition = new Vector2(79.35f, 0f);
    int pausePressedCount = 0;
    private bool hasScrolled = false;

    private LevelLoader levelLoader = null;
    private AudioManager audioManager = null;
    private bool isRestarting = false;
    private bool isGoingBackToMainMenu = false;

    private Coroutine buttonClickCoroutine = null;

    //stun Delay value
    private const float stunStopTimeDelay = 0.1f;
    private const float stunStopTimeDuration = 0.2f;
    private const float gameCompleteMenuDelay = 0.1f;

    public void Update()
    {
        if(ComposeNotPaused)
        {
            gameIsPaused = false;
        }
        
        if(gameIsPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {   
            Time.timeScale = 1f;
        }
    }

    private void Awake()
    {
        pauseMenu = GameObject.Find("[PauseMenu]");
        pauseMenuUI = GameObject.Find("[PauseMenu]").transform.GetChild(0).gameObject;
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
        inputManager = GameObject.Find("[InputManager]").GetComponent<InputManager>();
        if(inputManager != null )
            inputManager.PlayerInputActions.Player.PauseGame.performed += ctx => audioManager.PlaySFX("PauseMenuPopUp");
        levelLoader = GameObject.Find("[LevelLoader]").GetComponent<LevelLoader>();
        audiomanager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
        playerMovement = GameObject.Find("[Player]").GetComponent<PlayerMovement>();

        //GameOverEventManager.current.onGameOverTriggerEnter += GameOverPause;
        originalPosition = pauseMenuUI.GetComponent<RectTransform>().anchoredPosition;
        pauseMenuUI.SetActive(false);
        pauseMenuRectTransform = pauseMenuUI.GetComponent<RectTransform>();
    }
    private void Start()
    {
        gameIsPaused = false;
        isRestarting = false;
        isGoingBackToMainMenu = false;
    }

    public void DoPauseGame(InputAction.CallbackContext ctx)
    {
        if (buttonClickCoroutine == null)
        {
            if (!gameIsPaused)
            {
                if (pausePressedCount % 2 == 0)
                    PauseGame();
            }
            else
            {
                if (pausePressedCount % 2 != 0)
                    ResumeGame();
            }
            buttonClickCoroutine = StartCoroutine(IncrementCoroutine());
        }
    }

    private IEnumerator IncrementCoroutine()
    {
        ++pausePressedCount;
        yield return new WaitForSecondsRealtime(0.5f);
        buttonClickCoroutine = null;
    }

    public void ResumeGame()
    {
        inputManager.PlayerInputActions.UIs.Disable();
        inputManager.ToggleInputDevices(true);
        ComposeNotPaused = true;
        ScrollMenuDown(pauseMenuUI);
        audiomanager.ResumeBGM();
        if (!pauseMenuUI.activeSelf)
        {
            pauseMenuRectTransform.LeanSetPosX(880f);
        }
        gameIsPaused = false;
    }

    public void PauseGame()
    {
        inputManager.PlayerInputActions.UIs.Enable();
        inputManager.ToggleInputDevices(false);
        ComposeNotPaused = false;
        gameIsPaused = true;
        ScrollMenuUp(pauseMenuUI, scrollUpPosition);
        audiomanager.PauseBGM();
    }

    public void ScrollMenuUp(GameObject menuUI)
    {
        hasScrolled = true;
        if (hasScrolled)
        {
            LeanTween.init();
            menuUI.SetActive(true);
            LeanTween.move(menuUI.GetComponent<RectTransform>(), scrollUpPosition, 0.5f).setEaseOutQuart().setIgnoreTimeScale(true)
                .setOnComplete(ctx => hasScrolled = false);
        }
    }

    public void ScrollMenuUp(GameObject menuUI, Vector2 scrollUpPosition)
    {
        hasScrolled = true;
        if (hasScrolled)
        {
            LeanTween.init();
            menuUI.SetActive(true);
            LeanTween.move(menuUI.GetComponent<RectTransform>(), scrollUpPosition, 0.5f).setEaseOutQuart().setIgnoreTimeScale(true)
                .setOnComplete(ctx => hasScrolled = false);
        }
    }

    public void ScrollMenuDown(GameObject menuUI)
    {
        if(!hasScrolled)
        {
            LeanTween.init();
            LeanTween.moveY(menuUI.GetComponent<RectTransform>(), originalPosition.y, 0.3f).setEaseOutCubic().setIgnoreTimeScale(true)
            .setOnComplete(() => menuUI.SetActive(false));
            hasScrolled = true;
        }
    }

    public void LevelCompletePause(GameObject levelCompleteUI)
    {
        //pauseMenu.SetActive(false);
        StartCoroutine(WaitASecond());
        levelCompleteUI.SetActive(true);
        ComposeNotPaused = false;
        gameIsPaused = true;
        ScrollMenuUp(levelCompleteUI, Vector2.zero);
    }

    public void GameOverPause(GameObject GameOverUI)
    {
        GameOverUI.SetActive(true);
        pauseMenu.SetActive(false);
        ComposeNotPaused = false;
        gameIsPaused = true;
        ScrollMenuUp(GameOverUI, Vector2.zero);
        audiomanager.PauseBGM();
        inputManager.InputDisabled = true;
    }

    public void RestartGame()
    {
        if(!isRestarting && !isGoingBackToMainMenu)
        {   
            levelLoader.LoadCurrentScene();
            gameIsPaused = true;
            inputManager.PlayerInputActions.Disable();
            //BaseHouseDurability.instance.ResetDurability();
        }
        isRestarting = true;
    }

    public void ReturnToMainMenu()
    {
        if(!isGoingBackToMainMenu && !isRestarting)
        {
            levelLoader.ReturnToMainMenu();
        }
        isGoingBackToMainMenu = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator WaitASecond(){
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(gameCompleteMenuDelay);
        Time.timeScale = 1f;
        yield break;
    }
}
