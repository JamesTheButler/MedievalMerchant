using Common.UI.Elements;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Common.Utility
{
    public sealed class InputMapEnforcer : MonoBehaviour
    {
        private void Awake()
        {
            var playerInput = FindAnyObjectByType<PlayerInput>();
            // needed due to bug in unity where the default action map is not actually the only one that is on
            // we have to toggle back and forth to jiggle it into the correct state..
            playerInput?.SwitchCurrentActionMap(ActionMap.UI);
            playerInput?.SwitchCurrentActionMap(ActionMap.Gameplay);
        }
    }
}