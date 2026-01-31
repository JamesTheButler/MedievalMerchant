using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Common.UI.Elements
{
    public sealed class DynamicPanelInputBlocker : InitializableBehavior
    {
        [SerializeField, Required]
        private DynamicPanel dynamicPanel;

        private PlayerInput _playerInput;

        public override void Initialize()
        {
            _playerInput = FindAnyObjectByType<PlayerInput>();

            dynamicPanel.Opened += OnPanelOpened;
            dynamicPanel.Closed += OnPanelClosed;
        }

        private void OnPanelOpened()
        {
            _playerInput?.SwitchCurrentActionMap(ActionMap.UI);
        }

        private void OnPanelClosed()
        {
            _playerInput?.SwitchCurrentActionMap(ActionMap.Gameplay);
        }
    }
}