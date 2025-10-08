using System;
using Data.Player.Retinue.Config;
using NaughtyAttributes;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Player.Retinue.UI
{
    public sealed class CompanionLevelUI : MonoBehaviour
    {
        private const string NotUnlockableTooltip = "Unlock previous levels first.";

        public event Action<CompanionType, int> UnlockRequested;

        [SerializeField, Required]
        private Button unlockButton;

        [SerializeField, Required]
        protected TooltipHandler tooltip;

        private CompanionLevelData _levelData;
        private string _unlockableTooltip;
        private string _unlockedTooltip;
        private CompanionType _companionType;
        private int _level;

        private void Awake()
        {
            unlockButton.onClick.AddListener(OnUnlockButtonClicked);
        }

        public void Setup(int levelIndex, CompanionLevelData levelData, CompanionType companionType)
        {
            _level = levelIndex;
            _levelData = levelData;
            _companionType = companionType;
            SetUpTooltipStrings();
            SetUnlocked(false);
            SetUnlockable(false);
        }

        private void SetUpTooltipStrings()
        {
            _unlockableTooltip = $"Level {_level}: {_levelData.Cost} coin\n-----\n{_levelData.Description}";
            _unlockedTooltip = $"Level {_level}\n-----\n{_levelData.Description}";
        }

        private void OnUnlockButtonClicked()
        {
            UnlockRequested?.Invoke(_companionType, _level);
        }

        public void SetUnlocked(bool unlocked)
        {
            unlockButton.gameObject.SetActive(!unlocked);

            tooltip.SetTooltip(unlocked
                ? _unlockedTooltip
                : _unlockableTooltip);
        }

        public void SetUnlockable(bool unlockable)
        {
            unlockButton.interactable = unlockable;

            tooltip.SetTooltip(unlockable
                ? _unlockableTooltip
                : NotUnlockableTooltip);
        }
    }
}