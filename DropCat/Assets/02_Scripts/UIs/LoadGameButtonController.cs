using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameButtonController : MonoBehaviour
{
    private AudioManager audioManager;
    private Button button = null;
    [SerializeField] private Sprite deactivatedImage = null;
    [SerializeField] private Sprite activatedImage = null;

    private void Awake()
    {
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
        button = this.GetComponent<Button>();
    }

    public void OnButtonPointed()
    {
        LeanTween.scale(gameObject, new Vector3(0.1f, 0.1f, 1f), 0.1f).setEase(LeanTweenType.easeOutSine)
            .setIgnoreTimeScale(true);
        PlayTickSound();
    }

    public void OnButtonDeselected()
    {
        LeanTween.scale(gameObject, new Vector3(0.06f, 0.06f, 1f), 0.1f).setEase(LeanTweenType.easeOutSine)
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

    public void OnActivated()
    {
        button.image.sprite = activatedImage;
        button.enabled = true;
    }

    public void OnDeactivated()
    {
        button.image.sprite = deactivatedImage;
        button.enabled = false;
    }
}
