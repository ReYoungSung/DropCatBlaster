using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMessageBehaviour : MonoBehaviour
{
    protected RectTransform rectTransform = null;

    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }

    public IEnumerator ScrollMessageOut(Vector2 localFrom, Vector2 localTo, float time, float maintain)
    {
        rectTransform = this.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = localFrom;
        rectTransform.LeanMoveLocal(localTo, time).setEaseOutCirc();

        yield return new WaitForSeconds(maintain);

        rectTransform.LeanMoveLocal(localFrom, time).setEaseInSine();
        yield break;
    }

    public void ScrollMessageOut(Vector2 localFrom, Vector2 localTo, float time)
    {
        rectTransform = this.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = localFrom;
        rectTransform.LeanMoveLocal(localTo, time).setEaseOutCirc();
    }
}