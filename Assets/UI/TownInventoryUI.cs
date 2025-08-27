using System;
using Data.Configuration;
using Data.Towns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class TownInventoryUI : InventoryUI
    {
        [SerializeField]
        private TMP_Text townNameText;

        [SerializeField]
        private TMP_Text developmentScoreText;

        [SerializeField]
        private TMP_Text developmentTrendText;

        [SerializeField]
        private Image developmentTrendIcon;

        [SerializeField]
        private Button upgradeButton;

        private Town _boundTown;

        private readonly Lazy<GrowthTrendConfig> _growthConfig =
            new(() => ConfigurationManager.Instance.GrowthTrendConfig);

        public void Bind(Town town)
        {
            UnBindTown();

            if (town == null) return;

            base.Bind(town.Inventory);

            _boundTown = town;
            TownUpgrade();
            UpdateDevelopmentScore();
            UpdateDevelopmentTrend();
            _boundTown.TierChanged += TownUpgrade;
            _boundTown.DevelopmentScoreChanged += UpdateDevelopmentScore;
            _boundTown.DevelopmentTrendChanged += UpdateDevelopmentTrend;

            upgradeButton.onClick.AddListener(() => _boundTown.Upgrade());
        }

        public void UnBindTown()
        {
            upgradeButton.onClick.RemoveAllListeners();

            if (_boundTown == null) return;

            _boundTown.TierChanged -= TownUpgrade;
            _boundTown.DevelopmentScoreChanged -= UpdateDevelopmentScore;
            _boundTown.DevelopmentTrendChanged -= UpdateDevelopmentTrend;
            _boundTown = null;
        }

        private void TownUpgrade()
        {
            townNameText.text = $"{_boundTown.Name} ({_boundTown.Tier.ToRomanNumeral()})";

            foreach (var good in _boundTown.Production)
            {
                InventoryCells[good].SetIsProduced(true);
            }
        }

        private void UpdateDevelopmentScore()
        {
            developmentScoreText.text = $"{_boundTown.DevelopmentScore}";
        }

        private void UpdateDevelopmentTrend()
        {
            var sign = _boundTown.DevelopmentTrend > 0 ? "+" : "";
            developmentTrendText.text = $"{sign}{_boundTown.DevelopmentTrend}%";

            var growthConfig = _growthConfig.Value;
            var growthTrend = growthConfig.GetTrend(_boundTown.DevelopmentTrend);
            developmentTrendIcon.sprite = growthConfig.ConfigData[growthTrend].Icon;
        }
    }
}