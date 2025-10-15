using System;
using Common.UI;
using Features.Player.Retinue.Config;
using NaughtyAttributes;
using UI;
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

        private CompanionType _companionType;
        private int _level;
        private bool _isUnlocked, _isUpgraded;


        private void Awake()
        {
            unlockButton.onClick.AddListener(OnUnlockButtonClicked);
        }

        public void Setup(int levelIndex, CompanionType companionType)
        {
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
            unlockButton.gameObject.SetActive(!isUpgraded);

            tooltip.SetTooltip(new CompanionLevelTooltip.Data(_companionType, _level, _isUnlocked, _isUpgraded));
        }

        public void SetUnlocked(bool unlocked)
        {
            _isUnlocked = unlocked;
            unlockButton.interactable = unlocked;

            tooltip.SetTooltip(new CompanionLevelTooltip.Data(_companionType, _level, _isUnlocked, _isUpgraded));
        }
    }
}