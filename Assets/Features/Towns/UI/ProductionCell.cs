using System;
using Common.UI.Elements;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.UI
{
    public sealed class ProductionCell : InventoryCellBase
    {
        public enum State
        {
            Hidden = 0,
            Locked = 1,
            Upgradeable = 2,
            Active = 3
        }

        public event Action UnlockButtonClicked;

        [SerializeField, Required]
        private CanvasGroup rootCanvasGroup;

        [SerializeField, Required]
        private Button upgradeButton;

        [SerializeField, Required]
        private GameObject lockGroup;

        public int Index { get; set; }

        private State _currentState = State.Hidden;

        private void Awake()
        {
            upgradeButton.onClick.AddListener(() => UnlockButtonClicked?.Invoke());
            SetStateInternal(State.Hidden);
        }

        public void SetState(State state)
        {
            if (_currentState == state)
                return;
            // find diff between current and target state and roll-out button changes one step at a time
            var currentValue = (int)_currentState;
            var targetValue = (int)state;

            var step = currentValue < targetValue ? +1 : -1;

            while (currentValue != targetValue)
            {
                currentValue += step;
                var nextState = (State)currentValue;
                SetStateInternal(nextState);
                _currentState = nextState;
            }

            _currentState = state;
        }

        private void SetStateInternal(State state)
        {
            switch (state)
            {
                case State.Hidden:
                    rootCanvasGroup.alpha = 0f;
                    rootCanvasGroup.blocksRaycasts = false;
                    break;
                case State.Locked:
                    rootCanvasGroup.alpha = 1f;
                    rootCanvasGroup.blocksRaycasts = true;
                    lockGroup.SetActive(true);
                    upgradeButton.gameObject.SetActive(false);
                    break;
                case State.Upgradeable:
                    lockGroup.SetActive(false);
                    upgradeButton.gameObject.SetActive(true);
                    break;
                case State.Active:
                    upgradeButton.gameObject.SetActive(false);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void InvokeUnlockButtonClicked()
        {
            tooltipHandler.SetEnabled(false);
            UnlockButtonClicked?.Invoke();
        }
    }
}