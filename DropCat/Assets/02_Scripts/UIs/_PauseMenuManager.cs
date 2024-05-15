using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
using System;

public enum PausePageTogglingState
{
    Disabled, Enabled
}

public class _PauseMenuManager :  MonoBehaviour
{
    [SerializeField] private GameObject[] pausePages = null;
    [SerializeField] private GameObject[] pausePagesButtons = null;
    GameObject clickObject = null;
    private int menuPointer = 0;
    private _PlayerInputActions PlayerInputActions;
    private PausePageTogglingState pausePageToggling = PausePageTogglingState.Enabled;
    public PausePageTogglingState PausePageToggling { get { return pausePageToggling; } set { pausePageToggling = value; } }
    private float buttonToggleXPos = 328f;
    private Vector2 initialPos = new Vector2(540f, -1050f);
    private AudioManager audioManager = null;

    private void Awake()
    {
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
        PlayerInputActions = GameObject.Find("[InputManager]").GetComponent<InputManager>().PlayerInputActions;
        //PlayerInputActions.UIs.NavigateMenuLeft.performed += ctx => SwitchPointIntoButtons(ctx, menuPointer);
        //PlayerInputActions.UIs.ContinueDialogue.performed += ctx => ClickMenu(ctx);

       // PlayerInputActions.UIs.NavigateMenuUp.performed += ctx => PlayPauseMenuSwapSound(ctx);
        //PlayerInputActions.UIs.NavigateMenuDown.performed += ctx => PlayPauseMenuSwapSound(ctx);
        DisableAllPages();
    }

    private void PlayPauseMenuSwapSound()
    {
        if(pausePageToggling == PausePageTogglingState.Enabled)
        {
            audioManager.PlaySFX("PauseMenuSwapped");
        }
    }

    private void Update()
    {
        EnablePage(menuPointer);
    }

    public void EnablePage(int index)
    {
        for(int i = 0; i < pausePages.Length; i++)
        {
            if (i == index)
                pausePages[i].SetActive(true);
            else
                pausePages[i].SetActive(false);
        }
    }

    public void ClickMenu()
    {
        clickObject = EventSystem.current.currentSelectedGameObject;
        menuPointer = Array.IndexOf(pausePagesButtons, clickObject);
        PlayPauseMenuSwapSound();
    }

    private void SwitchPointIntoButtons(InputAction.CallbackContext ctx, int menuPointer)
    {
        if(pausePageToggling == PausePageTogglingState.Enabled)
        {
            if (0 < menuPointer)
            {
                pausePages[menuPointer].transform.GetChild(1).gameObject.SetActive(true);
                LeanTween.moveX(this.GetComponent<RectTransform>(), buttonToggleXPos, 0.2f).setEaseOutQuart().setIgnoreTimeScale(true);
                pausePageToggling = PausePageTogglingState.Disabled;
            }
        }
    }

    private void PointMenu(int menuIndex)
    {
        GameObject pointed = pausePages[menuIndex];
        pointed.SetActive(true);
    }

    private void ResetButtonState()
    {
        pausePageToggling = PausePageTogglingState.Enabled;
        menuPointer = 0;
        EnablePage(0);
    }

    private void OnEnable()
    {
        LeanTween.init(400);
    }

    private void OnDisable()
    {
        this.gameObject.GetComponent<RectTransform>().LeanSetPosX(79.35f);
        ResetButtonState();
    }

    public void ListUpSelections()
    {
        foreach(GameObject button in transform)
        {
            button.SetActive(true);
        }
    }

    public void DisableAllPages()
    {
        for(int i = 0; i < pausePages.Length; i++)
            pausePages[i].SetActive(false);
        menuPointer = 0;
    }
}
