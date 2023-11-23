using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using System;

public class LoadGameMenuManager : MonoBehaviour
{
    private int menuPointer = 0;
    GameObject clickObject = null;
    [SerializeField] private GameObject loadGameMenu = null;
    [SerializeField] private GameObject tutorialButton = null;
    [SerializeField] private GameObject[] levelButtons = null;
    private _PlayerInputActions PlayerInputActions;
    private Vector2 initialPos = new Vector2(540f, -1050f);

    bool hasScrolled = false;

    private Vector2 originalPosition;
    private GameObject levelLoaderObj = null;
    private LevelLoader levelLoader = null;
    private bool isStarting = false;
    private int oneStarValue = 100;
    private Vector3 scrollPosition = new Vector3(60,-150f,0);

    private void Awake()
    {
        clickObject = null;
        PlayerInputActions = new _PlayerInputActions();
        PlayerInputActions.UIs.ResumeGame.performed += ctx => ScrollMenuDown();
        PlayerInputActions.UIs.ResumeGame.performed += ctx => OnReturnToTitle();
        originalPosition = initialPos;
        loadGameMenu.SetActive(false);

        levelLoaderObj = GameObject.Find("[LevelLoader]").gameObject;
        levelLoader = levelLoaderObj.GetComponent<LevelLoader>();
    }

    public void EnableLevelLoader(bool isEnabled)
    {
        levelLoaderObj.SetActive(isEnabled);
    }

    private void Start()
    {
        AddButtonsToList();
        InitializeButtons();
    }

    private void NavigateMenu(InputAction.CallbackContext ctx)
    {
        PointMenu(menuPointer);
    }

    private void PointMenu(int menuIndex)
    {
        GameObject pointed = levelButtons[menuIndex];
        pointed.GetComponent<LoadGameButtonController>().OnButtonPointed();
    }

    private void AddButtonsToList()
    {
        levelButtons = new GameObject[loadGameMenu.transform.childCount - 4];
        int i = 0;
        foreach(Transform childObj in loadGameMenu.transform)
        {
            if (childObj.gameObject.GetComponent<Button>() != null)
            {
                if (0 < i)
                {
                    levelButtons[i-1] = childObj.gameObject;
                }
                else
                {
                    tutorialButton = childObj.gameObject;
                }
                ++i;
            }
        }
    }

    private void InitializeButtons()
    {
        tutorialButton.GetComponent<LoadGameButtonController>().OnActivated();
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i < SaveData.Current.playerProfile.maxLevelCompleted)
            {
                levelButtons[i].GetComponent<LoadGameButtonController>().OnActivated();
                levelButtons[i].transform.GetChild(0).gameObject.GetComponent<Slider>().value = SaveData.Current.playerProfile.levelCompleteStarResults[i] * oneStarValue;
            }
            else if (i == SaveData.Current.playerProfile.maxLevelCompleted)
            {
                levelButtons[i].GetComponent<LoadGameButtonController>().OnActivated();
                levelButtons[i].transform.GetChild(0).gameObject.GetComponent<Slider>().value = 0;
            }
            else
            {
                levelButtons[i].GetComponent<LoadGameButtonController>().OnDeactivated();
                levelButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void ClickMenu()
    {
        clickObject = EventSystem.current.currentSelectedGameObject;
        int menuNum = Array.IndexOf(levelButtons, clickObject);
        OnClickLoadSceneButton(menuNum);
    }

    public void ClickTutorial()
    {
        if (!isStarting)
        {
            PlayerInputActions.Disable();
            levelLoaderObj.SetActive(true);
            levelLoaderObj.GetComponent<LevelLoader>().LoadIndexedScene(2);
            loadGameMenu.SetActive(false);
            isStarting = true;
        }
    }

    private void OnClickLoadSceneButton(int menuNum)
    {
        PlayerInputActions.Disable();
        levelLoaderObj.SetActive(true);
        LoadSelectedLevel(menuNum);
        loadGameMenu.SetActive(false);
    }

    public void LoadSelectedLevel(int levelNum)
    {
        if (!isStarting)
        {
            levelLoaderObj.GetComponent<LevelLoader>().LoadIndexedScene(levelNum + 3);
            isStarting = true;
        }
    }

    public void OnReturnToTitle()
    {
        this.GetComponent<_MainMenuManager>().enabled = true;
    }

    public void ScrollMenuUp()
    {
        hasScrolled = true;
        if (hasScrolled)
        {
            LeanTween.init();
            loadGameMenu.SetActive(true);
            LeanTween.moveLocal(loadGameMenu, scrollPosition, 0.5f).setEaseOutQuart().setIgnoreTimeScale(true)
                .setOnComplete(ctx => hasScrolled = false);
            PlayerInputActions.Enable();
        }
    }

    public void ScrollMenuDown()
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
