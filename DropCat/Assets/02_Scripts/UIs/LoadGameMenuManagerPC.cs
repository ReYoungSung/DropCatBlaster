using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LoadGameMenuManagerPC : MonoBehaviour
{
    [SerializeField] private GameObject loadGameMenu;
    [SerializeField] private GameObject[] week01Buttons = null;

    private int menuPointer = 0;

    private _PlayerInputActions PlayerInputActions;
    private PausePageTogglingState pausePageToggling = PausePageTogglingState.Enabled;
    public PausePageTogglingState PausePageToggling { get { return pausePageToggling; } set { pausePageToggling = value; } }
    private float buttonToggleXPos = 328f;
    private Vector2 initialPos = new Vector2(540f, -1050f);
    private AudioManager audioManager = null;

    bool hasScrolled = false;

    private Vector2 originalPosition;
    private GameObject levelLoaderObj = null;
    private LevelLoader levelLoader = null;
    private bool isStarting = false;

    private void Awake()
    {
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
        PlayerInputActions = new _PlayerInputActions();
        PlayerInputActions.UIs.ResumeGame.performed += ctx => ScrollMenuDown();
        PlayerInputActions.UIs.NavigateMenuLeft.performed += ctx => NavigateMenuLeft(ctx);
        PlayerInputActions.UIs.NavigateMenuRight.performed += ctx => NavigateMenuRight(ctx);
        PlayerInputActions.UIs.ResumeGame.performed += ctx => OnReturnToTitle(ctx);
        PlayerInputActions.UIs.ContinueDialogue.performed += ctx => ClickMenu(ctx);
        originalPosition = initialPos;
        loadGameMenu.SetActive(false);

        levelLoaderObj = GameObject.Find("[LevelLoader]").gameObject;
        levelLoader = levelLoaderObj.GetComponent<LevelLoader>();
    }

    public void LoadSelectedLevel(int levelNum)
    {
        if(!isStarting)
        {
            levelLoaderObj.GetComponent<LevelLoader>().LoadIndexedScene(levelNum + 2);
            isStarting = true;
        }
    }

    public void EnableLevelLoader(bool isEnabled)
    {
        levelLoaderObj.SetActive(isEnabled);
    }

    private void Start()
    {
        InitializeButtons();
        Debug.Log("levelloader "+levelLoader.MaxLevelOpened);
    }

    private void InitializeButtons()
    {
        for (int i = 0; i < levelLoader.MaxScenebuildIndex - 2; i++)
        {
            if(i <= levelLoader.MaxLevelOpened)
            {
                week01Buttons[i].GetComponent<LoadGameButtonController>().OnActivated();
            }
            else
            {
                week01Buttons[i].GetComponent<LoadGameButtonController>().OnDeactivated();
            }
        }
    }

    private void ClickMenu(InputAction.CallbackContext ctx)
    {
        OnClickLoadSceneButton();
    }

    private void OnClickLoadSceneButton()
    {
        PlayerInputActions.Disable();
        levelLoaderObj.SetActive(true);
        LoadSelectedLevel(menuPointer);
        loadGameMenu.SetActive(false);
    }

    private void OnReturnToTitle(InputAction.CallbackContext ctx)
    {
        this.GetComponent<_MainMenuManager>().enabled = true;
    }

    private void NavigateMenuLeft(InputAction.CallbackContext ctx)
    {
        if (pausePageToggling == PausePageTogglingState.Enabled)
        {
            // ���� ��ư
            GameObject aux = week01Buttons[menuPointer];
            aux.GetComponent<LoadGameButtonController>().OnButtonDeselected();

            --menuPointer;
            if (menuPointer > levelLoader.MaxLevelOpened)
            {
                menuPointer = 0;
            }
            else if (menuPointer < 0)
            {
                menuPointer = levelLoader.MaxLevelOpened;
            }
            PointMenu(menuPointer);
        }
    }

    private void NavigateMenuRight(InputAction.CallbackContext ctx)
    {
        if (pausePageToggling == PausePageTogglingState.Enabled)
        {
            // ���� ��ư
            GameObject aux = week01Buttons[menuPointer];
            aux.GetComponent<LoadGameButtonController>().OnButtonDeselected();

            ++menuPointer;
            if (menuPointer > levelLoader.MaxLevelOpened)
            {
                menuPointer = 0;
            }
            else if (menuPointer < 0)
            {
                menuPointer = levelLoader.MaxLevelOpened;
            }
            PointMenu(menuPointer);
        }
    }

    private void SwitchPointIntoButtons(InputAction.CallbackContext ctx, int menuPointer)
    {
        if (pausePageToggling == PausePageTogglingState.Enabled)
        {
            if (0 < menuPointer)
            {
                week01Buttons[menuPointer].transform.GetChild(1).gameObject.SetActive(true);
                LeanTween.moveX(this.GetComponent<RectTransform>(), buttonToggleXPos, 0.2f).setEaseOutQuart().setIgnoreTimeScale(true);
                pausePageToggling = PausePageTogglingState.Disabled;
            }
        }
    }

    private void PointMenu(int menuIndex)
    {
        GameObject pointed = week01Buttons[menuIndex];
        pointed.GetComponent<LoadGameButtonController>().OnButtonPointed();
    }

    private void ResetButtonState()
    {
        pausePageToggling = PausePageTogglingState.Enabled;
        menuPointer = 0;
    }

    private void OnEnable()
    {
        LeanTween.init(400);
        PointMenu(0);
    }

    private void OnDisable()
    {
        ResetButtonState();
    }

    public void ListUpSelections()
    {
        foreach (GameObject button in transform)
        {
            button.SetActive(true);
        }
    }

    public void DisableAllPages()
    {
        for (int i = 0; i < week01Buttons.Length; i++)
            week01Buttons[i].SetActive(false);
    }

    public void ScrollMenuUp()
    {
        hasScrolled = true;
        if (hasScrolled)
        {
            LeanTween.init();
            loadGameMenu.SetActive(true);
            LeanTween.moveLocal(loadGameMenu, Vector3.zero, 0.5f).setEaseOutQuart().setIgnoreTimeScale(true)
                .setOnComplete(ctx => hasScrolled = false);
            PlayerInputActions.Enable();

        }
    }

    private void ScrollMenuDown()
    {
        if (!hasScrolled)
        {
            LeanTween.init();
            LeanTween.moveY(loadGameMenu, originalPosition.y, 0.3f).setEaseOutCubic().setIgnoreTimeScale(true)
            .setOnComplete(() => loadGameMenu.SetActive(false));
            hasScrolled = true;
            PlayerInputActions.Disable();
        }
    }
}
