using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Spine.Unity;

public class PopUpMessageBehaviour : MonoBehaviour
{
    private TextMeshProUGUI textMeshUI;
    private string messageContents = null;
    private float tweeningTime = 1f;
    public float TweeningTime { get { return tweeningTime; } set { tweeningTime = value; } }
    private float holdingTime = 1.5f;
    private Timer timer = null;
    private bool isTimered = false;
    public bool IsTimered { set { isTimered = value; } }

    private PopUpMessage popUpMessage = new PopUpMessage();
    private GameObject buttonPressAdvice = null;
    [SerializeField] private GameObject buttonPressAdvice_Keyboard = null;
    [SerializeField] private GameObject buttonPressAdvice_Gamepad = null;
    private Coroutine coroutine = null;
    private AudioManager audioManager = null;
    private RectTransform rectTransform = null;
    private SkeletonGraphic skeletonGraphic = null;
    private bool displayingAdviceNeeded = true;

    private void Awake()
    {
        textMeshUI = this.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        textMeshUI.text = messageContents;
        timer = this.GetComponent<Timer>();
        timer.TriggerTime = holdingTime + tweeningTime;
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
        rectTransform = this.GetComponent<RectTransform>();
        skeletonGraphic = this.transform.GetChild(4).gameObject.GetComponent<SkeletonGraphic>();
    }

    private void Start()
    {
        skeletonGraphic.enabled = false;
    }

    private void Update()
    {
        textMeshUI.text = messageContents;
    }

    public void PopsUpMessage(float from, float to)
    {
        Vector2 pos =  this.transform.localPosition;
        this.transform.localPosition = new Vector2(pos.x, from);
        LeanTween.init(1600);
        LeanTween.moveLocalY(this.gameObject, to, tweeningTime).setEaseOutCirc().setIgnoreTimeScale(true);
        //coroutine = StartCoroutine(timer.CoroutineTimer(1.8f, delegate { PopUpButtonPressAdvice(device); }));
        audioManager.PlaySFX("PopUpMessage_Sound");
    }

    public void PopsUpMessage()
    {
        LeanTween.init(1600);
        rectTransform.LeanMoveY(-422f, tweeningTime).setEaseOutCirc().setIgnoreTimeScale(true);
        timer.StartTicking();
        coroutine = StartCoroutine(timer.CoroutineTimer(1.8f, delegate { PopUpButtonPressAdvice(); }));
        audioManager.PlaySFX("PopUpMessage_Sound");
    }

    public void PopUpButtonPressAdvice()
    {
        if(displayingAdviceNeeded)
        {
            skeletonGraphic.enabled = true;
            skeletonGraphic.AnimationState.SetAnimation(0, "animation", true);
        }
    }
        
    public void PullsBackButtonPressAdvice()
    {
        skeletonGraphic.enabled = false;
    }

    public void ToggleButtonPressAdvice(bool buttonOn)
    {
        displayingAdviceNeeded = buttonOn;
    }

    public void StartHoldingMessage()
    {
        timer.StartTicking();
    }

    public void PullsBackMessage()
    {
        rectTransform.LeanMoveY(422f, tweeningTime).setEaseInCirc().setIgnoreTimeScale(true);
    }

    public void SwitchContents(string contents)
    {
        messageContents = contents;
        audioManager.PlaySFX("PopUpMessage_Sound");
    }
}
