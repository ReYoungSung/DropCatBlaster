/*
 * This code is deprecated due to the unnecessity of 
 * swiping punch behaviour.
 * 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    private InputManager inputManager = null;
    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;

    [SerializeField] private float minimumDistance = 0.3f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)]
    private float directionThreshold = 0.9f;
    private string punchDirection = "CENTER";
    public string PunchDirection { get { return punchDirection; } }

    private void Awake()
    {
        inputManager = this.GetComponent<InputManager>();
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeTouch;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeTouch;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeTouch(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if(Vector3.Distance(startPosition, endPosition) > minimumDistance &&
                (endTime - startTime) < maximumTime)
        {
            Vector2 direction = (endPosition - startPosition).normalized;
            UpdatePunchDirection(direction);
        }
    }

    private void UpdatePunchDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            punchDirection = "UP";
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            punchDirection = "DOWN";
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            punchDirection = "LEFT";
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            punchDirection = "RIGHT";
        }
        else
        {
            punchDirection = "CENTER";
        }
    }
}
*/