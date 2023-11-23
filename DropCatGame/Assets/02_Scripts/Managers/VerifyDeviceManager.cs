using UnityEngine;
using UnityEngine.InputSystem;

public static class VerifyDeviceManager
{
    public static string VerifyCurrentDevice()
    {
        if(Gamepad.current != null)
        {
            return "Gamepad";
        }
        else if(Keyboard.current != null)
        {
            return "Keyboard";
        }
        else
        {
            return "Mobile";
        }
    }
}
