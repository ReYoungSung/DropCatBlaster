using UnityEngine;
using UnityEngine.InputSystem;

public class ConnectedDeviceManager : MonoBehaviour
{
    private void Start()
    {
        InputSystem.onDeviceChange +=
            (device, change) =>
            {
                switch (change)
                {
                    case InputDeviceChange.Added:
                        break;
                    case InputDeviceChange.Reconnected:
                        break;
                    case InputDeviceChange.Disconnected:
                        break;
                    case InputDeviceChange.Removed:
                    default:
                        break;
                }
            };
    }
}
