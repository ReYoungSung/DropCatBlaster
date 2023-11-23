using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class LockCameraY : CinemachineExtension
{
    [Tooltip("Lock the camera's Y position to this value")]
    [SerializeField] private float yPosition = 10;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            pos.y = yPosition;
            state.RawPosition = pos;
        }
    }
}

