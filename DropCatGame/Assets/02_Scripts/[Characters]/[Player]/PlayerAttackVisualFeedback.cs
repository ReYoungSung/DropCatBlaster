using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackVisualFeedback : MonoBehaviour
{
    private LineRenderer visualFeedback = null;
    //[SerializeField] private float feedbacklWidth = 0f;

    private void Awake()
    {
        visualFeedback = this.GetComponent<LineRenderer>();
    }

    private void Start()
    {
        visualFeedback.SetPosition(1, this.transform.position);
    }

    public void DisplayVisualFeedback(bool toggleVal)
    {
        visualFeedback.enabled = toggleVal;
    }

    public void UpdateVisualFeedback(Vector2 aimingDirection, float feedbackHeightMultiplier)
    {
        Vector2 aimingPosition = this.transform.position;
        visualFeedback.SetPosition(0, aimingPosition + aimingDirection * feedbackHeightMultiplier);
        visualFeedback.SetPosition(1, aimingPosition);
    }
}
