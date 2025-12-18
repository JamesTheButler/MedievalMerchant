using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Player.Retinue.UI
{
    public sealed class CompanionLevelUI : MonoBehaviour
    {
        public enum State
        {
            Locked,
            Unlockable,
            Unlocked,
        }

        public event Action<CompanionType, int> UnlockRequested;

        [SerializeField, Required]
        private Button unlockButton;

        [SerializeField, Required]
        private CompanionLevelTooltipHandler tooltip;

        [SerializeField, Required]
        private Sprite defaultIcon, completedIcon;

        [SerializeField, Required]
        private Image levelIcon;

        private CompanionType _companionType;
        private int _level;

        private void Awake()
        {
            unlockButton.onClick.AddListener(OnUnlockButtonClicked);
        }

        public void Setup(int levelIndex, CompanionType companionType)
        {
            _level = levelIndex;
            _companionType = companionType;
            SetState(State.Locked);
        }

        public void SetState(State state)
        {
            switch (state)
            {
                case State.Locked:
                    levelIcon.sprite = defaultIcon;
                    unlockButton.gameObject.SetActive(false);
                    break;
                case State.Unlockable: 
                    levelIcon.sprite = defaultIcon;
                    unlockButton.gameObject.SetActive(true);
                    break;
                case State.Unlocked: 
                    levelIcon.sprite = completedIcon;
                    unlockButton.gameObject.SetActive(false);
                    break;
            }
            
            var tooltipData = CreateTooltipData(state);
            tooltip.SetData(tooltipData);
        }
        
        private void OnUnlockButtonClicked()
        {
            UnlockRequested?.Invoke(_companionType, _level);
        }

        private CompanionLevelTooltip.Data CreateTooltipData(State state)
        {
            return new CompanionLevelTooltip.Data(
                _companionType,
                _level,
                state);
        }
    }
}