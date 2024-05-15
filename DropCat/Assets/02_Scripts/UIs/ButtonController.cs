using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
    }

    /*
    public void OnButtonPointed()
    {
        LeanTween.scale(gameObject, new Vector3(0.41f, 0.41f, 1f), 0.1f).setEase(LeanTweenType.easeOutSine)
            .setIgnoreTimeScale(true);
        PlayTickSound();
    }
    */

    public void OnButtonWorkInProgress()
    {
        RectTransform workInProgressText = this.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        workInProgressText.LeanScale(new Vector3(1, 1, 1f), 0.1f).setEase(LeanTweenType.easeOutBounce)
            .setIgnoreTimeScale(true).setOnComplete(PullsBackWorkInProgress);
    }

    private void PullsBackWorkInProgress()
    {
        RectTransform workInProgressText = this.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        workInProgressText.LeanScale(new Vector3(0f, 0f, 1f), 0.1f).setDelay(1.5f);
    }

    public void OnButtonDeselected()
    {
        LeanTween.scale(gameObject, new Vector3(0.2f, 0.2f, 1f), 0.1f).setEase(LeanTweenType.easeOutSine)
            .setIgnoreTimeScale(true);
    }

    public void PlaySelectionSound()
    {
        if(audioManager != null)
            audioManager.PlaySFX("MenuClick");
    }

    public void PlayTickSound()
    {
        if(audioManager != null)
            audioManager.PlaySFX("MenuSelectionTick");
    }

    public void PlayWorkInProgressSound()
    {
        if (audioManager != null)
            audioManager.PlaySFX("MenuCancelBack");
    }
}
