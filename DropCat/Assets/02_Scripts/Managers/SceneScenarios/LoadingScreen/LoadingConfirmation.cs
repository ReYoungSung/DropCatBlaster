using System.Collections;
using UnityEngine;
using Spine.Unity;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingConfirmation : MonoBehaviour
{
    private Chapter chapterLoadingConfirmation = null;
    public Chapter ChapterLoadingConfirmation { get { return chapterLoadingConfirmation; } }

    private VideoManager videoManager = null;
    [SerializeField] private GameObject currentChapterObj = null;
    private SkeletonGraphic skeletonGraphic = null;

    private void Awake()
    {
        if(IsTitleMenu())
        {
            GameObject mainMenuObj = GameObject.Find("MainMenu");
            skeletonGraphic = mainMenuObj.transform.GetChild(2).gameObject.GetComponent<SkeletonGraphic>();
            skeletonGraphic.enabled = false;
            mainMenuObj.transform.GetChild(3).gameObject.SetActive(false);
        }
        chapterLoadingConfirmation = new Chapter();
        currentChapterObj.SetActive(false);
    }

    private bool IsTitleMenu()
    {
        return SceneManager.GetActiveScene().buildIndex == 1;
    }

    private void InitializeVideoManager()
    {
        videoManager = GameObject.Find("[VideoManager]").GetComponent<VideoManager>();
        videoManager.ToggleVideoLayerGameObject(0, true);
        videoManager.ToggleVideoLayerGameObject(1, true);
        videoManager.ToggleVideoLayer(0, true);
        videoManager.ToggleVideoLayer(1, false);
    }

    private void Start()
    {
        InitializeVideoManager();
        videoManager.RefreshVideoFileArray();
        LoadSequence_Loadingconfirm();
        chapterLoadingConfirmation.StartChapter();
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void LoadSequence_Loadingconfirm()
    {
        Sequence loadingConfirmSeq = new Sequence("LoadingConfirm");
        Beat beatCache = new Beat();
        beatCache.AddEventsToBeatEnter(
                delegate
                {
                    StartCoroutine(LoadingConfirmSequence());
                }
            );
        loadingConfirmSeq.AddBeatToCurrent(beatCache);
        chapterLoadingConfirmation.AddSequenceToCurrent(loadingConfirmSeq);
    }

    public IEnumerator LoadingConfirmSequence()
    {
        Time.timeScale = 1f;
        VideoPlayerManager videoPlayer_01 = videoManager.GetVideoPlayerManager(1);
        videoPlayer_01 = videoPlayer_01.gameObject.GetComponent<VideoPlayerManager>();
        videoPlayer_01.PlayVideo(4, true);
        videoManager.ToggleVideoLayer(1, true);
        while (!videoPlayer_01.FinishedPlaying(4))
        {
            yield return new WaitForSeconds(0.5f);
            videoManager.GetVideoPlayerManager(0).ClearActiveFile();
            videoManager.ToggleVideoLayer(0, false);
            yield return null;
            if (videoPlayer_01.FinishedPlaying(4))
            {
                Time.timeScale = 1f;
                currentChapterObj.SetActive(true);
                videoManager.GetVideoPlayerManager(1).ClearActiveFile();
                videoManager.ToggleVideoLayer(1, false);
                if(IsTitleMenu())
                {
                    skeletonGraphic.enabled = true;
                    skeletonGraphic.AnimationState.SetAnimation(0, "TitleMenuAppears", false);
                    GameObject mainMenuObj = GameObject.Find("MainMenu");
                    mainMenuObj.transform.GetChild(3).gameObject.SetActive(true);
                }
                yield break;
            }
        }
    }
}