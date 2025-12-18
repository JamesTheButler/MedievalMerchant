using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Player.Retinue.UI
{
    public sealed class CompanionLevelUI : MonoBehaviour
    {
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
        private bool _isUnlocked, _isUpgraded, _isImplemented;

        private void Awake()
        {
            unlockButton.onClick.AddListener(OnUnlockButtonClicked);
        }

        public void Setup(int levelIndex, CompanionType companionType, bool isImplemented)
        {
            _isImplemented = isImplemented;
            _level = levelIndex;
            _companionType = companionType;
            SetUpgraded(false);
            SetUnlocked(false);
        }

        private void OnUnlockButtonClicked()
        {
            UnlockRequested?.Invoke(_companionType, _level);
        }

        public void SetUpgraded(bool isUpgraded)
        {
            _isUpgraded = isUpgraded;
            levelIcon.sprite = isUpgraded ? completedIcon : defaultIcon;
            unlockButton.gameObject.SetActive(!isUpgraded);

            var tooltipData = CreateTooltipData();
            tooltip.SetData(tooltipData);
        }

        public void SetUnlocked(bool unlocked)
        {
            _isUnlocked = unlocked;
            unlockButton.interactable = unlocked;

            var tooltipData = CreateTooltipData();
            tooltip.SetData(tooltipData);
        }

        private CompanionLevelTooltip.Data CreateTooltipData()
        {
            return new CompanionLevelTooltip.Data(
                _companionType,
                _level,
                _isUnlocked,
                _isUpgraded,
                _isImplemented);
        }
    }
}