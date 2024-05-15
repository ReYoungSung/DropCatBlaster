using System;
using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that focuses view to the given position
/// </summary>
public class CameraFocusOperation : MonoBehaviour
{
    [SerializeField] private GameObject gameOverEventObj = null;
    [SerializeField] private Transform baseHouseTransform = null;
    private CinemachineVirtualCamera virtualCameraComponent = null;
    private CameraConstraint cameraYContraint = null;

    private GameObject playerObj = null;
    private GameObject targetObject = null;
    public GameObject TargetObject { set { targetObject = value; } }

    private void Awake()
    {
        cameraYContraint = this.GetComponent<CameraConstraint>();
        virtualCameraComponent = this.GetComponent<CinemachineVirtualCamera>();
        playerObj = GameObject.Find("[Player]");
    }

    private void Update()
    {
        if(targetObject == null)
        {
            virtualCameraComponent.m_Follow = playerObj.transform;
            cameraYContraint.enabled = true;
        }
    }

    public void FocusCameraToBaseHouse()
    {
        FocusCameraToPosition(new Vector2(baseHouseTransform.position.x, 145f), 1f);
    }

    public void FocusCameraToObject(GameObject focusObject, float time)
    {
        cameraYContraint.enabled = false;
        LeanTween.move(this.gameObject, (Vector2)focusObject.transform.position, time).
            setOnComplete(TrackObject).
            setEaseOutCubic().setIgnoreTimeScale(true);
        TrackObject();
    }

    public void FocusCameraToPosition(Vector2 focusPosition, float time)
    {
        cameraYContraint.enabled = false;
        LeanTween.move(this.gameObject, focusPosition, time).
            setOnComplete(TrackObject).
            setEaseOutCubic().setIgnoreTimeScale(true);
    }

    private void TrackObject()
    {
        virtualCameraComponent.m_Follow = targetObject.transform;
    }
}
