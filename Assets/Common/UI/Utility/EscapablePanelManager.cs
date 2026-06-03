using System.Collections.Generic;
using Common.UI.Elements;
using Common.UI.Elements.Panels;
using Common.Utility;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Common.UI.Utility
{
    public sealed class EscapablePanelManager : MonoBehaviour
    {
        [SerializeField, Required]
        private PlayerInput playerInput;

        [SerializeField]
        private UnityEvent escapeFallthrough;

        [SerializeField]
        private List<DynamicPanel> trackedPanels;

        private readonly List<IOpenClosable> _activePanels = new();

        private void Awake()
        {
            foreach (var panel in trackedPanels)
            {
                panel.Opened += OnPanelOpened;
                continue;

                void OnPanelOpened()
                {
                    TrackPanel(panel);
                }
            }
        }

        public void OnEscapePressed(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            EscapeNext();
        }

        private void TrackPanel(DynamicPanel panel)
        {
            // if it's already managed, we just bring it to the top of the list.
            if (_activePanels.Contains(panel))
            {
                _activePanels.Remove(panel);
                _activePanels.Add(panel);
                return;
            }

            _activePanels.Add(panel);
            panel.Closed += OnPanelClosed;
            ToggleGameplayInputMap();

            return;

            void OnPanelClosed()
            {
                _activePanels.Remove(panel);
                panel.Closed -= OnPanelClosed;
                ToggleGameplayInputMap();
            }
        }

        private void EscapeNext()
        {
            if (_activePanels.Count <= 0)
            {
                escapeFallthrough.Invoke();
                return;
            }

            var lastIndex = _activePanels.Count - 1;
            var next = _activePanels[lastIndex];
            _activePanels.RemoveAt(lastIndex);
            next.Close();
        }

        private void ToggleGameplayInputMap()
        {
            var activeMap = _activePanels.IsEmpty() ? ActionMap.Gameplay : ActionMap.UI;
            playerInput.SwitchCurrentActionMap(activeMap);
        }
    }
}