using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick : MonoBehaviour
{
    private Touch touch;
    private Vector2 touchPos;
    private Vector2 originPos; 
    private Vector2 currentTouchPos;

    // 화면 넓이의 절반
    private float screenWidthHalf; 

    void Awake()
    {
        screenWidthHalf = Screen.width/2;
        originPos = this.gameObject.GetComponent<RectTransform>().anchoredPosition;
        currentTouchPos = originPos;
    }

    void Update()
    {
        Debug.Log(currentTouchPos);
        if (Input.touchCount <= 0)
        {
            if(touch.phase == TouchPhase.Ended)
            {
                this.gameObject.GetComponent<RectTransform>().anchoredPosition = originPos;
            }
        }
        else
        {
            touch = Input.GetTouch(0);
            touchPos = Input.GetTouch(0).position;
            if(touch.phase == TouchPhase.Began)
            {
                this.gameObject.GetComponent<RectTransform>().position = touchPos;
            }
        }

        // if (currentTouchPos.x < screenWidthHalf)
        // {
        //     currentTouchPos = touchPos;
        //     this.gameObject.GetComponent<RectTransform>().anchoredPosition = currentTouchPos;
        // }
        // else
        // {
        //     currentTouchPos = originPos;
        //     this.gameObject.GetComponent<RectTransform>().anchoredPosition = currentTouchPos;
        // }
    }
}
