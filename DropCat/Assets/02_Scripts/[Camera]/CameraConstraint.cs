using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class CameraConstraint : CinemachineExtension
{
    [Tooltip("Lock the camera's Y position to this value")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform baseHouseTransform;
    private float cameraHeightThreshold = 145f;
    public float CameraHeightThreshold { get { return cameraHeightThreshold; } }

    private void Start()
    {
        this.transform.position = new Vector3(65f, cameraHeightThreshold, -10f);
    }

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            Vector3 pos = state.RawPosition;
            pos.x = baseHouseTransform.position.x;
            if (cameraHeightThreshold < playerTransform.position.y)
            {
                pos.y = playerTransform.position.y;
            }
            else
            {
                pos.y = cameraHeightThreshold;
            }
            state.RawPosition = pos;
        }
    }
}