using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using System;

public class StartPageManager : PageManager
{
    [SerializeField] private GameObject startPageButtonHolder = null;
    [SerializeField] private GameObject[] startPageButtons = null;
    private int menuPointer = 0;
    GameObject clickObject = null;

    private _PlayerInputActions PlayerInputActions;
    [SerializeField] private _PauseMenuManager pauseMenuManager;

    private void Awake()
    {
        pauseMenuManager = pauseMenuManager.gameObject.GetComponent<_PauseMenuManager>();
    }

    private void Start()
    {
        PlayerInputActions = GameObject.Find("[InputManager]").GetComponent<InputManager>().PlayerInputActions;
        //PlayerInputActions.UIs.NavigateMenuLeft.performed += ctx => ResetButtonState();
        //PlayerInputActions.UIs.NavigateMenuRight.performed += ctx => SwitchPointOutToPages(ctx);
        //PlayerInputActions.UIs.ContinueDialogue.performed += ctx => ClickMenu(ctx);
    }

    public void ClickMenu()
    {
        clickObject = EventSystem.current.currentSelectedGameObject;
        menuPointer = Array.IndexOf(startPageButtons, clickObject);
        //startPageButtons[menuPointer].GetComponent<Button>().onClick.Invoke();
    }

    // private void PointMenu(int menuIndex)
    // {
    //     GameObject pointed = startPageButtons[menuIndex];
    //     pointed.GetComponent<ButtonController>().OnButtonPointed();
    // }

    // private void ResetButtonState()
    // {
    //     if(pauseMenuManager.PausePageToggling == PausePageTogglingState.Disabled)
    //     {
    //         foreach (GameObject page in startPageButtons)
    //         {
    //             page.GetComponent<ButtonController>().OnButtonDeselected();
    //             menuPointer = 0;
    //         }
    //         startPageButtons[menuPointer].GetComponent<ButtonController>().OnButtonPointed();
    //     }
    // }

    // private void OnDisable()
    // {
    //     pauseMenuManager.PausePageToggling = PausePageTogglingState.Enabled;
    //     menuPointer = 0;
    //     foreach (GameObject button in startPageButtons)
    //     {
    //         button.GetComponent<ButtonController>().OnButtonDeselected();
    //     }
    // }

    // public void SetActiveAll(bool isActive)
    // {
    //     startPageButtonHolder.SetActive(isActive);
    // }

    // private void SwitchPointOutToPages(InputAction.CallbackContext ctx)
    // {
    //     if (pauseMenuManager.PausePageToggling == PausePageTogglingState.Disabled)
    //     {
    //         foreach (GameObject button in startPageButtons)
    //         {
    //             button.GetComponent<ButtonController>().enabled = false;
    //         }
    //     }
    //     startPageButtons[menuPointer].GetComponent<ButtonController>().OnButtonDeselected();
    //     LeanTween.moveX(pauseMenuManager.gameObject.GetComponent<RectTransform>(), 0f, 0.2f).setEaseOutQuart().setIgnoreTimeScale(true);
    //     pauseMenuManager.PausePageToggling = PausePageTogglingState.Enabled;
    // }
}
