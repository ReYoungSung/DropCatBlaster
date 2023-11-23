using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;


public class _MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject creditButton, startButton, quitButton; 
    private GameObject[] mainMenu = new GameObject[3];
    private int menuPointer = 1;

    private GameObject levelLoader;
    //[SerializeField] private DeveloperCredit devCredits = null;

    private Coroutine coroutine = null;

    private bool pointerConstrained = false;
    public bool PointerConstrained { get { return pointerConstrained; } set { pointerConstrained = value; } }
    
    private int creditClickCount = 0;
    [SerializeField] private VideoPlayer creditPlayer;
    [SerializeField] private GameObject levelLoadMenuObj = null;
    private _PlayerInputActions menuInputActions = null;

    public _PlayerInputActions MenuInputActions { get { return menuInputActions; } }
    private AudioManager audioManager = null;

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        mainMenu[0] = creditButton;
        mainMenu[1] = startButton;
        mainMenu[2] = quitButton;

        menuInputActions = new _PlayerInputActions();
        menuInputActions.UIs.NavigateMenuLeft.performed += ctx => NavigateMenuLeft(ctx);
        menuInputActions.UIs.NavigateMenuRight.performed += ctx => NavigateMenuRight(ctx);
        menuInputActions.UIs.ContinueDialogue.performed += ctx => ClickMenu(ctx);

        //devCredits = devCredits.gameObject.GetComponent<DeveloperCredit>();
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
        creditPlayer = creditPlayer.gameObject.GetComponent<VideoPlayer>();
        this.GetComponent<LoadGameMenuManager>().enabled = false;
        Cursor.visible = false;
    }

    private void Start()
    {
        audioManager.PlayBGM("[OTR]_MainMenu");
    }

    private void Update()
    {
        if(pointerConstrained)
        {
            menuPointer = 0;
        }

        /*
        if(devCredits.gameObject.activeSelf)
        {
            if(menuInputActions.Touch.TouchAnyWhereToContinue.triggered)
            {
                creditPlayer.clip = null;
                pointerConstrained = false;
                mainMenu[0].transform.parent.gameObject.SetActive(true);
                devCredits.gameObject.SetActive(false);
            }
        }
        */
    }

    public void OnStartButtonPressed()
    {
        this.GetComponent<LoadGameMenuManager>().enabled = true;
        this.GetComponent<LoadGameMenuManager>().ScrollMenuUp();
        menuInputActions.Disable();
        ToggleMainMenu(false);
    }

    /*
    public void DisplayDeveloperCredit()
    {
        devCredits.gameObject.SetActive(true);
        devCredits.DisplayDevCredits();
    }
    */

    private void ClickMenu(InputAction.CallbackContext ctx)
    {
        mainMenu[menuPointer].GetComponent<Button>().onClick.Invoke();
    }

    private void NavigateMenuLeft(InputAction.CallbackContext ctx)
    {
        // ���� ��ư
        GameObject aux = mainMenu[menuPointer];
        //aux.GetComponent<ButtonController>().OnButtonDeselected();

        --menuPointer;
        if(menuPointer > mainMenu.GetLength(0)-1)
        {
            menuPointer = 0;
        }
        else if (menuPointer < 0)
        {
            menuPointer = mainMenu.GetLength(0) - 1;
        }
        //PointMenu(menuPointer);
    }

    private void NavigateMenuRight(InputAction.CallbackContext ctx)
    {
        // ���� ��ư
        GameObject aux = mainMenu[menuPointer];
        //aux.GetComponent<ButtonController>().OnButtonDeselected();

        ++menuPointer;
        if (menuPointer > mainMenu.GetLength(0) - 1)
        {
            menuPointer = 0;
        }
        else if (menuPointer < 0)
        {
            menuPointer = mainMenu.GetLength(0) - 1;
        }
        //PointMenu(menuPointer);
    }

    /*
    private void PointMenu(int menuIndex)
    {
        GameObject pointed = mainMenu[menuIndex];
        pointed.GetComponent<ButtonController>().OnButtonPointed();
    }
    */

    private void ResetButtonState()
    {
        foreach (GameObject button in mainMenu)
        {
            if(button != null)
            {
                //button.GetComponent<ButtonController>().OnButtonDeselected();
                menuPointer = 1;
            }
        }
    }

    private void OnEnable()
    {
        LeanTween.init(400);
        menuInputActions.Enable();
        menuPointer = 1;
        //mainMenu[menuPointer].GetComponent<ButtonController>().OnButtonPointed();
    }

    private void OnDisable()
    {
        menuInputActions.Disable();
        //ResetButtonState();
    }

    public void ToggleMainMenu(bool isEnabled)
    {
        this.enabled = isEnabled;
    }
}
