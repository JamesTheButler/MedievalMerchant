using UnityEngine.InputSystem;

namespace Common
{
    public static class MouseExtension
    {
        public static bool WasAnyKeyPressedThisFrame(this Mouse mouse)
        {
            return mouse.leftButton.wasPressedThisFrame ||
                   mouse.rightButton.wasPressedThisFrame ||
                   mouse.middleButton.wasPressedThisFrame ||
                   mouse.forwardButton.wasPressedThisFrame ||
                   mouse.backButton.wasPressedThisFrame;
        }
    }
}