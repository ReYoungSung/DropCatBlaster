using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

[RequireComponent(typeof(SaveManager))]
public class LevelLoader : MonoBehaviour
{
    private Coroutine loadingCoroutine = null;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider slider;
    [SerializeField] private TMPro.TextMeshProUGUI loadingProgress;
    private GameObject buttonIndicator = null;
    [SerializeField] private GameObject buttonIndicator_Gamepad = null;

    private _PlayerInputActions PlayerInputActions;

    private float progressCache = 0;

    private float lerpTime = 1f;
    private float currentLerpTime;
    private bool isLoading = false;
    public bool IsLoading { get { return isLoading; } }

    private SaveManager saveManager = null;
    private LoadingConfirmation loadingConfirmation = null;
    private int maxSceneBuildIndex = 0;
    public int MaxScenebuildIndex { get { return maxSceneBuildIndex; } }

    private int maxLevelCompleted;
    public int MaxLevelOpened { get { return maxLevelCompleted; } }

    [Header("Managers")]
    [SerializeField] private VideoManager videoManager;
    public int GetCurrentLevelIndex { get { return SceneManager.GetActiveScene().buildIndex; } }
    private bool loadingLoopStarted = false;

    private void Awake()
    { 
        loadingScreen.SetActive(false);
        loadingProgress.gameObject.SetActive(false);
        videoManager = GameObject.Find("[VideoManager]").GetComponent<VideoManager>();
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            loadingConfirmation = this.GetComponent<LoadingConfirmation>();
        }
        PlayerInputActions = new _PlayerInputActions();
        
        buttonIndicator = buttonIndicator_Gamepad;
        
        buttonIndicator_Gamepad.SetActive(false);
        buttonIndicator.SetActive(false);
        saveManager = this.GetComponent<SaveManager>();
        saveManager.OnLoadLevel();
        maxLevelCompleted = SaveData.Current.playerProfile.maxLevelCompleted;
        maxSceneBuildIndex = SceneManager.sceneCountInBuildSettings - 1 ;
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            videoManager.InitializeVideoLayerObjs(this.gameObject, true);
        }
        else
        {
            videoManager.InitializeVideoLayerObjs(this.gameObject, false);
        }
        videoManager.ToggleVideoLayer(0, false);
    }

    public void EnablePlayerInputAction()
    {
        PlayerInputActions.Enable();
    }

    public void DisablePlayerInputAction()
    {
        PlayerInputActions.Disable();
    }

    private void Start()
    {
        videoManager.RefreshVideoFileArray();
    }

    public void Dev_LoadMainMenu()
    {
        StartCoroutine(LoadScene(1));
    }

    public void LoadLevelByNumber(int levelNum)
    {
        if (loadingCoroutine == null)
            loadingCoroutine = StartCoroutine(LoadScene(levelNum+2));
        else
        {
            loadingCoroutine = null;
            loadingCoroutine = StartCoroutine(LoadScene(levelNum+2));
        }
    }

    public void ReturnToMainMenu()
    {
        if (!isLoading)
        {
            if (loadingCoroutine == null)
                loadingCoroutine = StartCoroutine(LoadScene(1));
            else
            {
                loadingCoroutine = null;
                loadingCoroutine = StartCoroutine(LoadScene(1));
            }
            isLoading = true;
        }
    }

    public void LoadIndexedScene(int index)
    {
        if(!isLoading)
        {
            if (loadingCoroutine == null)
                loadingCoroutine = StartCoroutine(LoadScene(index));
            else
            {
                loadingCoroutine = null;
                loadingCoroutine = StartCoroutine(LoadScene(index));
            }
            isLoading = true;
        }
    }

    public void LoadCurrentScene()
    {
        if (!isLoading)
        {
            if (loadingCoroutine == null)
                loadingCoroutine = StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
            else
            {
                loadingCoroutine = null;
                loadingCoroutine = StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
            }
            isLoading = true;
        }
    }

    public void LoadNextScene()
    {
        if (!isLoading)
        {
            if (loadingCoroutine == null)
            {
                if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings-1)
                {
                    loadingCoroutine = StartCoroutine(LoadScene(1));
                }
                else
                {
                    loadingCoroutine = StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
                    GameInfoLogger.InitializeTrial();
                }
            }
            else
            {
                loadingCoroutine = null;
                if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings-1)
                {
                    loadingCoroutine = StartCoroutine(LoadScene(1));
                }
                else
                {
                    loadingCoroutine = StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
                }
            }
            isLoading = true;
        }
    }

    public void LoadContinuingScene()
    {
        if (!isLoading)
        {
            if (loadingCoroutine == null)
            {
                if (maxLevelCompleted == 0)
                {
                    loadingCoroutine = StartCoroutine(LoadScene(2));
                }
                else
                {
                    loadingCoroutine = StartCoroutine(LoadScene(maxLevelCompleted + 3));
                }
            }
            else
            {
                loadingCoroutine = null;
                if (maxLevelCompleted == 0)
                {
                    loadingCoroutine = StartCoroutine(LoadScene(2));
                }
                else
                {
                    loadingCoroutine = StartCoroutine(LoadScene(maxLevelCompleted + 3));
                }
            }
            isLoading = true;
        }
    }

    protected virtual IEnumerator LoadScene(int levelIndex)
    {
        /*
        if (loadingConfirmation != null)
        {
            loadingConfirmation.ToggleLoadingVideoLayer(0, true);
        }
        */
        Coroutine levelStartInputWait = null;
        PlayerInputActions.Disable();
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Single);
        loadingOperation.allowSceneActivation = false;
        VideoPlayerManager videoLayer_00 = videoManager.GetVideoPlayerManager(0);
        VideoPlayerManager videoLayer_01 = videoManager.GetVideoPlayerManager(1);
        videoManager.ToggleVideoLayer(0, false);
        videoLayer_01.PlayVideo(0);
        while (!videoLayer_01.FinishedPlaying(0))
        {
            if (0.3d < videoLayer_01.GetCurrentVideoTime() && videoLayer_01.GetCurrentVideoTime() < 0.4d)
            {
                videoManager.ToggleVideoLayer(1, true);
            }
            if (1.45d < videoLayer_01.GetCurrentVideoTime() && videoLayer_01.GetCurrentVideoTime() < 2.3d)
            {
                videoLayer_00.PlayVideo(1, true, false);
                videoManager.ToggleVideoLayer(0, true);
                loadingScreen.SetActive(true);
                loadingProgress.gameObject.SetActive(true);
            }
            yield return null;
            if (videoLayer_01.FinishedPlaying(0))
            {
                loadingLoopStarted = true;
            }
        }

        while (loadingLoopStarted)
        {
            videoLayer_00.PlayVideo(2, false, true);
            if (!loadingOperation.isDone)
            {
                if (loadingOperation.progress < 0.9f)
                {
                    progressCache = Mathf.Clamp01(loadingOperation.progress / 0.9f);
                    slider.value = progressCache * 100;
                    loadingProgress.text = progressCache * 100f + "%";
                }
                if (loadingOperation.progress >= 0.9f)
                {
                    slider.value = 100;
                    loadingProgress.text = null;
                    buttonIndicator.SetActive(true);
                    PlayerInputActions.Enable();
                    if(levelStartInputWait == null)
                        levelStartInputWait = StartCoroutine(WaitForLevelStartInput(loadingOperation));
                }
            }
            yield return null;
        }
        
    }

    private IEnumerator WaitForLevelStartInput(AsyncOperation loadingOperation)
    {
        while (!PlayerInputActions.UIs.ContinueDialogue.triggered)
        {
            yield return null;
            if (PlayerInputActions.UIs.ContinueDialogue.triggered)
            {
                buttonIndicator.SetActive(false);
                //loadingProgress.text = "Punch!"; 
                loadingOperation.allowSceneActivation = true;
                isLoading = false;
                yield break;
            }
        }
    }

    public void SaveLevel(int starResult)
    {
        if (SaveData.Current.playerProfile.levelCompleteStarResults[SceneManager.GetActiveScene().buildIndex - 3] <= starResult)
        {
            SaveData.Current.playerProfile.levelCompleteStarResults[SceneManager.GetActiveScene().buildIndex - 3] = starResult;
        }
        //SaveData.Current.playerProfile.levelCompleteStarResults[SaveData.Current.playerProfile.maxLevelCompleted] = 0;

        if(saveManager != null)
            saveManager.OnSaveLevel();
    }

    private float LoadingProgressLerp(float start, float end)
    {
        //increment timer once per frame
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        //lerp!
        float perc = currentLerpTime / lerpTime;
        perc = perc * perc * perc * (perc * (6f * perc - 15f) + 10f);
        return Mathf.Lerp(start, end, perc);
    }

    public void IncreaseMaxLevelCompleted()
    {
        if (HasNotReachedLastLevel() && !IsOnLevelReached())
        {
            ++SaveData.Current.playerProfile.maxLevelCompleted;
        }
    }

    private bool HasNotReachedLastLevel()
    {
        return maxLevelCompleted < maxSceneBuildIndex - 2;
    }

    private bool IsOnLevelReached()
    {
        return SceneManager.GetActiveScene().buildIndex - 2 <= maxLevelCompleted; 
    }

    private void OnEnable()
    {
        PlayerInputActions.Enable();
    }

    private void OnDisable()
    {
        PlayerInputActions.Disable();
    }
}
