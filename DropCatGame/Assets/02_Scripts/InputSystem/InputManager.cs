using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private string swipeDirection = "CENTER";

    private _PlayerInputActions playerInputActions;
    private bool inputDisabled = false;
    public bool InputDisabled { get { return inputDisabled; } set { inputDisabled = value; } }
    private FloatingJoystick moveJoystick = null;
    public FloatingJoystick MoveJoystick { get { return moveJoystick; } }
    private GameObject joyStickImage = null;
    public _PlayerInputActions PlayerInputActions { get { return playerInputActions; } }

    private void Awake()
    {
        playerInputActions = new _PlayerInputActions();
        Transform gamePlayCanvas = GameObject.Find("[UI] GamePlayCanvas").transform;
        joyStickImage = gamePlayCanvas.GetChild(1).gameObject;
        if(1 < SceneManager.GetActiveScene().buildIndex)
        {
            moveJoystick = gamePlayCanvas.GetChild(5).gameObject.GetComponent<FloatingJoystick>();
        }
    }

    private void Update()
    {
        if(!inputDisabled)
        {
            FakeJoystickImageEnable();
        }
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex <= 1)
        {
            inputDisabled = true;
            playerInputActions.Player.Disable();
            playerInputActions.UIs.Enable();
        }
        else
        {
            inputDisabled = false;
            playerInputActions.Player.Enable();
            playerInputActions.UIs.Disable();
        }

        //playerInputActions.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        //playerInputActions.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    public void ToggleInputDevices(bool toggleVal)
    {
        moveJoystick.enabled = toggleVal;
    }
    
    private void OnEnable()
    {
        playerInputActions.Enable();
        moveJoystick.enabled = true;
        inputDisabled = false;
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
        inputDisabled = true;
    }

    public bool IsMovingJoystick()
    {
        return moveJoystick.Direction != Vector2.zero;
    }
    
    private void FakeJoystickImageEnable()
    {
        if(moveJoystick.PointerIsDown)
        {
            joyStickImage.SetActive(false);
        }
        else
        {
            joyStickImage.SetActive(true);
        }
    }

    /*
    private void DetectAttackSwipeDirection()
    {
        if (moveJoystick.Horizontal > moveJoystick.HandleRange)
        {
            swipeDirection = "RIGHT";
        }
        else if (moveJoystick.Horizontal < -moveJoystick.HandleRange)
        {
            swipeDirection = "LEFT";
        }
        else if (moveJoystick.Vertical < -moveJoystick.HandleRange)
        {
            swipeDirection = "DOWN";
        }
        else if (moveJoystick.Vertical > moveJoystick.HandleRange)
        {
            swipeDirection = "UP";
        }
        else
        {
            swipeDirection = "CENTER";
        }
    }
    */
}