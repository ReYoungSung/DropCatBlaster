using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitializationLogoScreen : MonoBehaviour
{
    private _PlayerInputActions PlayerInputActions;
    // 비디오 레이어 순서 = 배열 인덱스
    [Header("Managers")]
    [SerializeField] private VideoManager videoManager;

    [SerializeField] private TMPro.TextMeshProUGUI screenTouchAdvice = null;
    private AsyncOperation loadingOperation;

    private void Awake()
    {
        PlayerInputActions = GameObject.Find("[InputManager]").GetComponent<InputManager>().PlayerInputActions;
        videoManager = GameObject.Find("[VideoManager]").GetComponent<VideoManager>();
        screenTouchAdvice.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(RenderVideoLayer(0, 0, true));
        StartCoroutine(RenderLoadingLayer(1, 1));
    }

    private IEnumerator RenderVideoLayer(int layer, int fileIndex, bool countinueToNext)
    {
        VideoPlayerManager videoLayer_00 = videoManager.GetVideoPlayerManager(layer);
        videoLayer_00.PlayVideo(fileIndex);
        if (countinueToNext)
        {
            while (!videoLayer_00.FinishedPlaying(fileIndex))
            {
                yield return null;
                if (videoLayer_00.FinishedPlaying(fileIndex))
                {
                    loadingOperation.allowSceneActivation = true;
                    loadingOperation = null;
                }
            }
        }
        else
        {
            yield return null;
        }
    }

    private IEnumerator RenderLoadingLayer(int layer, int fileIndex)
    {
        loadingOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        loadingOperation.allowSceneActivation = false;
        while (!loadingOperation.isDone)
        {
            yield return new WaitForSeconds(1f);
            if (loadingOperation.progress >= 0.9f)
            {
                screenTouchAdvice.gameObject.SetActive(true);
                while (!PlayerInputActions.Touch.TouchAnyWhereToContinue.triggered)
                {
                    yield return null;
                    if (PlayerInputActions.Touch.TouchAnyWhereToContinue.triggered)
                    {
                        loadingOperation.allowSceneActivation = true;
                    }
                }
            }
        }
    }
    private void OnDisable()
    {
        videoManager.ResetVideoLayers();
    }
}
