using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSwapControl : MonoBehaviour
{
    [SerializeField] private Sprite initialSprite = null;
    [SerializeField] private Sprite pressedSprite = null;

    public void SwapButtonImage(int currentStatus)
    {
        if(currentStatus == 0)
        {
            this.GetComponent<Image>().sprite = initialSprite;
        }
        else if(currentStatus == 1)
        {
            this.GetComponent<Image>().sprite = pressedSprite;
        }
    }
}
