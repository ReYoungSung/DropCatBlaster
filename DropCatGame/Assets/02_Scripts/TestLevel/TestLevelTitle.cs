using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLevelTitle : MonoBehaviour
{
    private Image titleImage = null;
    private RectTransform rectTransform = null;
    private AudioManager audioManager = null;

    private void Awake()
    {
        titleImage = this.GetComponent<Image>();
        rectTransform = this.GetComponent<RectTransform>();
        audioManager = GameObject.Find("[AudioManager]").GetComponent<AudioManager>();
    }

    public void TitleDisplay(float textDisplayTime)
    {
        StartCoroutine(PopUpTitle(textDisplayTime));
    }

    private IEnumerator PopUpTitle(float textDisplayTime)
    {
        audioManager.PlayBGM("Coin");
        LeanAlpha(titleImage, 1, 0.8f);
        rectTransform.LeanMoveLocalY(0f, 1f).setEaseOutCubic().setIgnoreTimeScale(true);
        yield return new WaitForSeconds(textDisplayTime);
        LeanAlpha(titleImage, 0, 0.5f);
        rectTransform.LeanMoveLocalY(87f, 1.5f).setEaseInCubic().setIgnoreTimeScale(true);
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
}
