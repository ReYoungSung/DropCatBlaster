using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMenuControl : MonoBehaviour
{
    private GameObject buttonPressAdvice = null;
    private string connectedDeviceInfo = null;
    private GameObject connectedDeviceMessageObj = null;
    [SerializeField] private GameObject[] buttonPressAdviceList = null;

    [SerializeField] private GameObject videoManagerObj = null;
    private VideoManager videoManager = null;
    private AudioManager audioManager = null;
    [SerializeField] private string mainMenuBGM = null;

    private void Awake()
    {
        videoManager = videoManagerObj.GetComponent<VideoManager>();
        videoManager.GetVideoPlayerManager(0).ClearActiveFile();
        videoManager.GetVideoPlayerManager(1).ClearActiveFile();
        Time.timeScale = 1;
        VerifyButtonPressAdviceInfo();
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
    }

    private void Start()
    {
        //RollText();
        audioManager.PlayBGM(mainMenuBGM);
    }

    private void VerifyButtonPressAdviceInfo()
    {
        buttonPressAdvice = buttonPressAdviceList[0];
    }


    private void RollText()
    {
        ScrollTextMessageBehaviour scrollTextMessageBehav = null;
        if (connectedDeviceInfo == "Keyboard")
        {
            scrollTextMessageBehav =
                buttonPressAdvice.transform.GetChild(0).GetComponent<ScrollTextMessageBehaviour>();
            scrollTextMessageBehav.SetText("Press E to Continue");
            scrollTextMessageBehav.ScrollMessageOut(new Vector2(1248f, -479f), new Vector2(648f, -479f), 0.4f);
            buttonPressAdvice.transform.GetChild(1).GetComponent<ScrollMessageBehaviour>().
                ScrollMessageOut(new Vector2(-1454f, -486f), new Vector2(-643f, -486f), 0.4f);
        }
        else
        {
            scrollTextMessageBehav =
                buttonPressAdvice.transform.GetChild(0).GetComponent<ScrollTextMessageBehaviour>();
            scrollTextMessageBehav.SetText("Press B to Continue");
            scrollTextMessageBehav.ScrollMessageOut(new Vector2(1248f, -479f), new Vector2(648f, -479f), 0.4f);
            buttonPressAdvice.transform.GetChild(1).GetComponent<ScrollMessageBehaviour>()
                .ScrollMessageOut(new Vector2(-1410f, -486f), new Vector2(-675f, -486f), 0.4f);
        }
    }
}
