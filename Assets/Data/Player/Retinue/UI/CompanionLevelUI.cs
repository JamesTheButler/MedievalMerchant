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

        public event Action<CompanionLevelUI> UnlockRequested;

        [SerializeField, Required] private Button unlockButton;
        [SerializeField, Required] private TooltipHandler tooltip;

        public int Index { get; private set; }

        private CompanionLevelData _levelData;

        private string _unlockableTooltip;
        private string _unlockedTooltip;

        private void Awake()
        {
            unlockButton.onClick.AddListener(OnUnlockButtonClicked);
        }

        public void Setup(int levelIndex, CompanionLevelData levelData)
        {
            Index = levelIndex;
            _levelData = levelData;
            SetUpTooltipStrings();
            SetUnlocked(false);
            SetUnlockable(false);
        }

        private void SetUpTooltipStrings()
        {
            _unlockableTooltip = $"Level {Index}\n-----\n{_levelData.Cost} coin\n{_levelData.Description}";
            _unlockedTooltip = $"Level {Index}\n-----\n{_levelData.Description}";
        }

        private void OnUnlockButtonClicked()
        {
            UnlockRequested?.Invoke(this);
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
            unlockButton.interactable = !unlockable;

            tooltip.SetTooltip(unlockable
                ? _unlockableTooltip
                : NotUnlockableTooltip);
        }
    }
}