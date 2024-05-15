using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperCredit : MonoBehaviour
{
    [SerializeField] private VideoManager videoManager;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private _MainMenuManager menuManager;

    private void Awake()
    {
        videoManager = GameObject.Find("[VideoManager]").GetComponent<VideoManager>();
        menuManager = menuManager.gameObject.GetComponent<_MainMenuManager>();
        mainMenu.SetActive(false);
    }

    private void Start()
    {
        videoManager.RefreshVideoFileArray();
    }

    public void DisplayDevCredits()
    {
        menuManager.PointerConstrained = true;
        mainMenu.SetActive(false);
        videoManager.RefreshVideoFileArray();
        VideoPlayerManager creditsLayer_00 = videoManager.GetVideoPlayerManager(2);
        creditsLayer_00.PlayVideo(3);
    }
}
