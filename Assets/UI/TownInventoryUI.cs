using Data;
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
        private Button upgradeButton;

        private Town _boundTown;

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
            developmentTrendText.text = $"{_boundTown.DevelopmentTrend}%";
        }
    }
}