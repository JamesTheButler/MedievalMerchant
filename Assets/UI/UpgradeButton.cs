using System;
using Data.Configuration;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class UpgradeButton : MonoBehaviour
    {
        public enum State
        {
            Hidden = 0, // button is not shown
            Disabled = 1, // button is shown but inactive
            Active = 2, // button is fully visible and interactable
        }

        [SerializeField, Required]
        private CanvasGroup canvasGroup;

        [SerializeField, Required]
        private TMP_Text costText;

        [SerializeField, Required]
        private Button button;

        [SerializeField, Required]
        private TooltipHandler tooltipHandler;

        public State ButtonState { get; private set; }
        public Button.ButtonClickedEvent OnClick => button.onClick;

        private readonly Lazy<Colors> _colors = new(() => ConfigurationManager.Instance.Colors);

        private float _cost;

        public void SetState(State state)
        {
            ButtonState = state;
            switch (state)
            {
                case State.Hidden:
                    Hide();
                    break;
                case State.Disabled:
                    Show();
                    button.interactable = false;
                    tooltipHandler.SetEnabled(true);
                    tooltipHandler.SetTooltip("Unlock previous upgrades first.");
                    break;
                case State.Active:
                    Show();
                    button.interactable = true;
                    tooltipHandler.SetEnabled(false);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void SetCost(int cost)
        {
            _cost = cost;
            costText.text = cost.ToString("N0");
        }

        public void Validate(float availableFunds)
        {
            var canAfford = availableFunds >= _cost;
            var color = canAfford ? _colors.Value.FontDark : _colors.Value.Bad;
            costText.color = color;
        }

        private void Hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        private void Show()
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }
}