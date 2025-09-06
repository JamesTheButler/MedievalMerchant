using UnityEngine.InputSystem;

public static class MouseExtension
{
    public static bool WasAnyKeyPressedThisFrame(this Mouse mouse)
    {
        return Mouse.current.leftButton.wasPressedThisFrame ||
               Mouse.current.rightButton.wasPressedThisFrame ||
               Mouse.current.middleButton.wasPressedThisFrame ||
               Mouse.current.forwardButton.wasPressedThisFrame ||
               Mouse.current.backButton.wasPressedThisFrame;
    }
}