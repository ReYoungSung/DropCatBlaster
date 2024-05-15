using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitializationLogoScreen : MonoBehaviour
{
    // 비디오 레이어 순서 = 배열 인덱스
    [Header("Managers")]
    private VideoManager videoManager;
    private VideoPlayerManager videoLayer_00;
    private AsyncOperation loadingOperation = null;

    [SerializeField] private TMPro.TextMeshProUGUI loadingText = null;

    private void Awake()
    {
        //PlayerInputActions = GameObject.Find("[InputManager]").GetComponent<InputManager>().PlayerInputActions;
        videoManager = GameObject.Find("[VideoManager]").GetComponent<VideoManager>();
        //screenTouchAdvice.gameObject.SetActive(false);
        loadingText = loadingText.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void Start()
    {
        loadingText.enabled = false;
        PlayVideo(0, 0);
        //StartCoroutine(RenderLoadingLayer(1, 1));
    }

    private void Update()
    {
        RenderVideoLayer(0, 0);
    }

    private void PlayVideo(int layer, int fileIndex)
    {
        videoLayer_00 = videoManager.GetVideoPlayerManager(0);
        videoLayer_00.PlayVideo(fileIndex);
    }

    private void RenderVideoLayer(int layer, int fileIndex)
    {
        if (1.5d < videoLayer_00.GetCurrentVideoTime())
        {
            if(loadingText.enabled == false)
            {
                loadingOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            }
            loadingText.enabled = true;
        }
        loadingText.text = "Loading..." + Mathf.FloorToInt(loadingOperation.progress * 100) + "%";
    }

    //private IEnumerator RenderLoadingLayer(int layer, int fileIndex)
    //{
    //    loadingOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    //    loadingOperation.allowSceneActivation = false;
    //    while (!loadingOperation.isDone)
    //    {
    //        yield return new WaitForSeconds(1f);
    //        if (loadingOperation.progress >= 0.9f)
    //        {
    //            screenTouchAdvice.gameObject.SetActive(true);
    //            while (!PlayerInputActions.Touch.TouchAnyWhereToContinue.triggered)
    //            {
    //                yield return null;
    //                if (PlayerInputActions.Touch.TouchAnyWhereToContinue.triggered)
    //                {
    //                    loadingOperation.allowSceneActivation = true;
    //                }
    //            }
    //        }
    //    }
    //}
    private void OnDisable()
    {
        videoManager.ResetVideoLayers();
    }
}
