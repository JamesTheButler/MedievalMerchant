using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Configuration;
using Data.Towns;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public sealed class TownInventoryUI : InventoryUI
    {
        [SerializeField]
        private UnityEvent<InventoryCell> inventoryCellClicked;

        [Header("Header UI Elements")]
        [SerializeField]
        private TMP_Text townNameText;

        [SerializeField]
        private DevelopmentSlider developmentScore;

        [SerializeField]
        private TMP_Text developmentTrendText;

        [SerializeField]
        private Image developmentTrendIcon;

        [SerializeField]
        private Button upgradeButton;

        [Header("Inventory UI Elements")]
        [SerializeField]
        private TMP_Text fundsText;

        [SerializeField]
        private TownInventorySection inventorySectionT1;

        [SerializeField]
        private TownInventorySection inventorySectionT2;

        [SerializeField]
        private TownInventorySection inventorySectionT3;

        private Town _boundTown;

        private Inventory _boundInventory;
        private Dictionary<Tier, TownInventorySection> _inventorySections;

        private readonly Lazy<GrowthTrendConfig> _growthConfig =
            new(() => ConfigurationManager.Instance.GrowthTrendConfig);

        public void Initialize()
        {
            _inventorySections = new Dictionary<Tier, TownInventorySection>
            {
                { Tier.Tier1, inventorySectionT1 },
                { Tier.Tier2, inventorySectionT2 },
                { Tier.Tier3, inventorySectionT3 },
            };

            foreach (var section in _inventorySections.Values)
            {
                section.CellClicked += InvokeCellClicked;
            }
        }

        public void Bind(Town town)
        {
            Unbind();

            if (town == null) return;

            BindTown(town);

            HideSection(Tier.Tier2);
            HideSection(Tier.Tier3);

            upgradeButton.onClick.AddListener(() => _boundTown.Upgrade());

            // makes sure that the UI is properly inflated after dynamic inventory creation
            Canvas.ForceUpdateCanvases();
        }

        private void BindTown(Town town)
        {
            _boundTown = town;

            BindInventory(_boundTown.Inventory);

            _boundTown.TierChanged += TownUpgrade;
            _boundTown.DevelopmentScoreChanged += UpdateDevelopmentScore;
            _boundTown.DevelopmentTrendChanged += UpdateDevelopmentTrend;

            TownUpgrade();
            UpdateDevelopmentScore();
            UpdateDevelopmentTrend();
        }

        private void BindInventory(Inventory inventory)
        {
            ResetInventorySections();

            inventory.GoodUpdated += OnGoodUpdated;
            inventory.FundsUpdated += OnFundsUpdated;

            foreach (var (good, amount) in inventory.Goods)
            {
                OnGoodUpdated(good, amount);
            }

            OnFundsUpdated(inventory.Funds);

            _boundInventory = inventory;
        }

        public void Unbind()
        {
            upgradeButton.onClick.RemoveAllListeners();

            UnbindTown();
            UnbindInventory();
        }

        private void UnbindTown()
        {
            if (_boundTown == null) return;

            _boundTown.TierChanged -= TownUpgrade;
            _boundTown.DevelopmentScoreChanged -= UpdateDevelopmentScore;
            _boundTown.DevelopmentTrendChanged -= UpdateDevelopmentTrend;
            _boundTown = null;
        }

        private void UnbindInventory()
        {
            fundsText.text = "0";

            ResetInventorySections();

            if (_boundInventory == null) return;

            _boundInventory.GoodUpdated -= OnGoodUpdated;
            _boundInventory.FundsUpdated -= OnFundsUpdated;
            _boundInventory = null;
        }

        private void ResetInventorySections()
        {
            foreach (var section in _inventorySections.Values)
            {
                section.Initialize();
            }
        }

        private void TownUpgrade()
        {
            var newTier = _boundTown.Tier;
            townNameText.text = $"{_boundTown.Name} ({newTier.ToRomanNumeral()})";
            ShowSection(newTier);
        }

        private void UpdateDevelopmentScore()
        {
            developmentScore.SetDevelopment(_boundTown.DevelopmentScore);
        }

        private void UpdateDevelopmentTrend()
        {
            var sign = _boundTown.DevelopmentTrend > 0 ? "+" : "";
            developmentTrendText.text = $"{sign}{_boundTown.DevelopmentTrend}%";

            var growthConfig = _growthConfig.Value;
            var growthTrend = growthConfig.GetTrend(_boundTown.DevelopmentTrend);
            developmentTrendIcon.sprite = growthConfig.ConfigData[growthTrend].Icon;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void HideSection(Tier tier)
        {
            _inventorySections[tier].gameObject.SetActive(false);
        }

        private void ShowSection(Tier tier)
        {
            _inventorySections[tier].gameObject.SetActive(true);
        }

        private void OnFundsUpdated(int funds)
        {
            fundsText.text = funds.ToString();
        }

        private void OnGoodUpdated(Good good, int amount)
        {
            var tier = ConfigurationManager.Instance.GoodsConfig.ConfigData[good].Tier;

            var isProduced = _boundTown?.Production.Contains(good) ?? false;
            _inventorySections[tier].UpdateGood(good, amount, isProduced);
        }

        private void InvokeCellClicked(InventoryCell cell)
        {
            inventoryCellClicked?.Invoke(cell);
        }
    }
}