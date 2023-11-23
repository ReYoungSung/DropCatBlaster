using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeJoyStickManager : MonoBehaviour
{
    private InputManager inputManager = null;
    private Image joyStickImage = null;
    [SerializeField] private Sprite[] SwipeUI;

    private void Awake()
    {
        inputManager = GameObject.Find("[InputManager]").GetComponent<InputManager>();
        joyStickImage = this.GetComponent<Image>();
    }

    private void Update()
    {
        SwapJoyStickSprite();
    }

    private void SwapJoyStickSprite()
    {
        if (inputManager.MoveJoystick.Horizontal > inputManager.MoveJoystick.HandleRange)
        {
            joyStickImage.sprite = SwipeUI[3];
        }
        else if (inputManager.MoveJoystick.Horizontal < -inputManager.MoveJoystick.HandleRange)
        {
            joyStickImage.sprite = SwipeUI[2];
        }
        else if (inputManager.MoveJoystick.Vertical < -inputManager.MoveJoystick.HandleRange)
        {
            joyStickImage.sprite = SwipeUI[1];
        }
        else if (inputManager.MoveJoystick.Vertical > inputManager.MoveJoystick.HandleRange)
        {
            joyStickImage.sprite = SwipeUI[0];
        }
        else
        {
            joyStickImage.sprite = SwipeUI[4];
        }
    }
}
