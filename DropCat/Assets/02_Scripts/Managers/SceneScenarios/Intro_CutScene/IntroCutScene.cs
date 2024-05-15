using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroCutScene : MonoBehaviour
{
    [SerializeField] private GameObject[] cloudGroup;
    [SerializeField] private Vector2[] spawnPoint;

    [SerializeField] private float textDisplayTime = 4.2f;
    [SerializeField] private float textDisplayInterval = 1.3f;
    [SerializeField] private float cloudDisplayInterval = 1.3f;
    private float buttonAdviceDisplayDelay = 6f;
    private Coroutine cloudCoroutine = null;
    private Coroutine textCoroutine = null;
    private Coroutine titleCoroutine = null;
    private Coroutine messageCoroutine = null;
    [SerializeField] private Animator cameraAnimator;
    private const string cameraPanDownClip = "[Camera] MoveToGround";
    private bool cameraPannedDown = false;

    private string[] text = new string[2];
    [SerializeField] private TMPro.TextMeshProUGUI textUI = null;

    [SerializeField] private GameObject mainMenuObj = null;
    [SerializeField] private Image titleImage = null;

    private bool canEnableMenu = false;

    private GameObject buttonPressAdvice = null;
    private string connectedDeviceInfo = null;
    [SerializeField] private GameObject connectedDeviceMessageObj = null;
    [SerializeField] private GameObject[] buttonPressAdviceList = null;

    [SerializeField] private GameObject videoManagerObj = null;
    private VideoManager videoManager = null;
    private GameObject mainMenuManager = null;
    private AudioManager audioManager = null;
    [SerializeField] private string mainMenuBGM = null;

    private void Awake()
    {
        mainMenuManager = GameObject.Find("[MainmenuManager]");
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
        videoManager = videoManagerObj.GetComponent<VideoManager>();
        videoManager.GetVideoPlayerManager(0).ClearActiveFile();
        videoManager.GetVideoPlayerManager(1).ClearActiveFile();
        Time.timeScale = 1;
        cameraAnimator = cameraAnimator.gameObject.GetComponent<Animator>();
        textUI = textUI.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        textUI.alpha = 0f;
        text[0] = "TeamGooCat presents";
        text[1] = "in association with\nHGU Capstone Program";

        mainMenuObj.SetActive(false);
        titleImage = mainMenuObj.transform.GetChild(0).GetComponent<Image>();
        titleImage.color = new Color(titleImage.color.r, titleImage.color.g, titleImage.color.b, 0f);

        VerifyButtonPressAdviceInfo();
    }

    private void Start()
    {
        audioManager.PlayBGM(mainMenuBGM);
        cloudCoroutine = StartCoroutine(cloudSpawning(4));
        textCoroutine = StartCoroutine(RollText(text));
        mainMenuManager.SetActive(false);
    }

    private IEnumerator RollText(string[] text)
    {
        foreach (string t in text)
        {
            textUI.text = t;
            StartCoroutine(DisplayText());
            yield return new WaitForSeconds(textDisplayTime);
        }

        yield return new WaitForSeconds(buttonAdviceDisplayDelay);
        ScrollTextMessageBehaviour scrollTextMessageBehav = null;
        if (connectedDeviceInfo == "Keyboard")
        {
            scrollTextMessageBehav = 
                buttonPressAdvice.transform.GetChild(0).GetComponent<ScrollTextMessageBehaviour>();
            scrollTextMessageBehav.SetText("");
            scrollTextMessageBehav.ScrollMessageOut(new Vector2(1248f, -479f), new Vector2(648f, -479f), 0.4f);
            buttonPressAdvice.transform.GetChild(1).GetComponent<ScrollMessageBehaviour>().
                ScrollMessageOut(new Vector2(-1454f, -486f), new Vector2(-643f, -486f), 0.4f);
        }
        else
        {
            scrollTextMessageBehav =
                buttonPressAdvice.transform.GetChild(0).GetComponent<ScrollTextMessageBehaviour>();
            scrollTextMessageBehav.SetText("");
            scrollTextMessageBehav.ScrollMessageOut(new Vector2(1248f, -479f), new Vector2(648f, -479f), 0.4f);
            buttonPressAdvice.transform.GetChild(1).GetComponent<ScrollMessageBehaviour>()
                .ScrollMessageOut(new Vector2(-1410f, -486f), new Vector2(-675f, -486f), 0.4f);
        }
    }

    private IEnumerator DisplayText()
    {
        LeanAlpha(textUI, 1, 0.5f);
        yield return new WaitForSeconds(textDisplayTime);
        LeanAlpha(textUI, 0, 0.5f);
        yield return null;
    }

    public static LTDescr LeanAlpha(TMPro.TextMeshProUGUI textMesh, float to, float time)
    {
        Color color = textMesh.color;
        LTDescr tween = LeanTween
            .value(textMesh.gameObject, color.a, to, time)
            .setOnUpdate((float value) => {
                color.a = value;
                textMesh.color = color;
            });
        return tween;
    }

    public static LTDescr LeanAlpha(Image image, float to, float time)
    {
        Color color = image.color;
        LTDescr tween = LeanTween
            .value(image.gameObject, color.a, to, time)
            .setOnUpdate((float value) => {
                color.a = value;
                image.color = color;
            });
        return tween;
    }

    private void updateValueExampleCallback(Color val)
    {
        textUI.color = val;
    }

    private IEnumerator cloudSpawning(int spawningCount)
    {
        for(int i = 0; i < spawningCount; ++i)
        {
            GameObject cloud = Instantiate(cloudGroup[i % 3], spawnPoint[i%2], Quaternion.identity);
            yield return new WaitForSeconds(cloudDisplayInterval);
            if(i == 1)
            {
                ScrollTextMessageBehaviour scrollTextMessageBehaviour =
                    connectedDeviceMessageObj.transform.GetChild(0).GetComponent<ScrollTextMessageBehaviour>();
                scrollTextMessageBehaviour.SetText("Main Device : Mobile" ); //임시로 해둠 원래는 connectedDeviceInfo 이거
                messageCoroutine =
                    StartCoroutine(scrollTextMessageBehaviour
                    .ScrollMessageOut(new Vector2(1196f, 410f), new Vector2(650f, 410f), 0.2f, 2f));
            }
        }
        cameraPannedDown = true;
        cameraAnimator.Play(cameraPanDownClip);
    }
    private void Update()
    {
        if (titleCoroutine == null)
            titleCoroutine = StartCoroutine(PanCameraToGround());
    }

    private IEnumerator PanCameraToGround()
    {
        if(cameraPannedDown)
        {
            cameraAnimator.SetBool("CameraToGround", true);
            while (!canEnableMenu)
            {
                yield return new WaitForSeconds(cameraAnimator.GetCurrentAnimatorStateInfo(0).length);
                canEnableMenu = true;
            }
        }
        if (canEnableMenu)
        {
            mainMenuObj.SetActive(true);
            mainMenuManager.SetActive(true);
            LeanAlpha(titleImage, 1, 0.5f);
        }
    }

    private void VerifyButtonPressAdviceInfo()
    {
        connectedDeviceInfo = VerifyDeviceManager.VerifyCurrentDevice();
        if(connectedDeviceInfo == "Gamepad")
        {
            buttonPressAdvice = buttonPressAdviceList[0];
        }
        else if(connectedDeviceInfo == "Keyboard")
        {
            buttonPressAdvice = buttonPressAdviceList[1];
        }
        connectedDeviceMessageObj = buttonPressAdviceList[1];
    }
}
