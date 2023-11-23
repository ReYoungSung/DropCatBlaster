using UnityEngine;

public class PauseButtonOperator : MonoBehaviour
{
    private Canvas uICanvas = null;
    private void Awake()
    {
        uICanvas = this.GetComponent<Canvas>();
    }
    public void AnimateButtonOut()
    {
        this.GetComponent<RectTransform>().LeanMoveLocalY(-300f, 0.2f).setEaseOutCirc().setIgnoreTimeScale(true);
        uICanvas.sortingOrder = 1;
        this.GetComponent<RectTransform>().LeanMoveLocalY(103f, 0.1f).setEaseOutCirc().setIgnoreTimeScale(true);
    }

    public void AnimateButtonIn()
    {
        this.GetComponent<RectTransform>().LeanMoveLocalY(-103f, 0.2f).setEaseOutCirc().setIgnoreTimeScale(true);
        uICanvas.sortingOrder = 0;
        this.GetComponent<RectTransform>().LeanMoveLocalY(300f, 0.1f).setEaseOutCirc().setIgnoreTimeScale(true);

    }
}
