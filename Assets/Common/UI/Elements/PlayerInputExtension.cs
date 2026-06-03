using UnityEngine.InputSystem;

namespace Common.UI.Elements
{
    public static class PlayerInputExtension
    {
        public static void SwitchCurrentActionMap(this PlayerInput playerInput, ActionMap map)
        {
            var mapId = map switch
            {
                ActionMap.Gameplay => "Gameplay",
                ActionMap.UI => "UI",
                _ => "Gameplay"
            };

            if (playerInput.currentActionMap.name == mapId)
                return;

            playerInput.SwitchCurrentActionMap(mapId);
        }
    }
}