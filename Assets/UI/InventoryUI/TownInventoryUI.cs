using System;
using AYellowpaper.SerializedCollections;
using Common;
using Data;
using Data.Configuration;
using Data.Towns;
using Data.Trade;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.InventoryUI
{
    public sealed class TownInventoryUI : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField]
        private UnityEvent<InventoryCellBase> inventoryCellClicked;

        [SerializeField]
        private UnityEvent<ProductionCell> upgradeButtonClicked;

        [SerializeField]
        private UnityEvent<Town> travelButtonClicked;

        [Header("Header UI Elements")]
        [SerializeField, Required]
        private TMP_Text townNameText;

        [SerializeField, Required]
        private DevelopmentSlider developmentScore;

        [SerializeField, Required]
        private TMP_Text developmentTrendText;

        [SerializeField, Required]
        private Image developmentTrendIcon;

        [Header("Inventory UI Elements")]
        [SerializeField, Required]
        private TMP_Text fundsText;

        [SerializeField, SerializedDictionary("Tier", "Section")]
        private SerializedDictionary<Tier, TownInventorySection> inventorySections;

        private Town _boundTown;
        private Inventory _boundInventory;

        private readonly Lazy<GrowthTrendConfig> _growthConfig =
            new(() => ConfigurationManager.Instance.GrowthTrendConfig);

        private readonly Lazy<GoodsConfig> _goodsConfig =
            new(() => ConfigurationManager.Instance.GoodsConfig);

        public void Initialize()
        {
            foreach (var section in inventorySections.Values)
            {
                section.UpgradeButtonClicked += productionCell => upgradeButtonClicked.Invoke(productionCell);
                section.InventoryCellClicked += productionCell => inventoryCellClicked.Invoke(productionCell);
            }
        }

        public void Bind(Town town)
        {
            Unbind();

            if (town == null) return;

            BindTown(town);

            // makes sure that the UI is properly inflated after dynamic inventory creation
            Canvas.ForceUpdateCanvases();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Upgrade()
        {
            _boundTown.Upgrade();
        }

        public void TravelHere()
        {
            travelButtonClicked?.Invoke(_boundTown);
        }

        private void BindTown(Town town)
        {
            _boundTown = town;

            BindInventory(_boundTown.Inventory);

            // don't invoke directly as we want to go through all tiers manually in the right order
            _boundTown.Tier.Observe(TownUpgrade, false);
            for (var tier = Tier.Tier1; tier <= _boundTown.Tier; tier++)
            {
                ShowSection(tier);
            }

            RefreshTownName(_boundTown.Tier);

            _boundTown.DevelopmentScore.Observe(UpdateDevelopmentScore);
            _boundTown.DevelopmentTrend.Observe(UpdateDevelopmentTrend);
            _boundTown.GrowthTrend.Observe(UpdateGrowthTrend);
        }

        private void BindInventory(Inventory inventory)
        {
            HideSection(Tier.Tier2);
            HideSection(Tier.Tier3);

            SetUpInventorySections();

            _boundTown.Producer.ProductionAdded += UnlockProducer;

            inventory.GoodUpdated += OnGoodUpdated;
            inventory.Funds.Observe(OnFundsUpdated);

            foreach (var (good, amount) in inventory.Goods)
            {
                OnGoodUpdated(good, amount);
            }

            _boundInventory = inventory;
        }

        public void Unbind()
        {
            UnbindTown();
            UnbindInventory();
        }

        private void UnbindTown()
        {
            if (_boundTown == null) return;

            _boundTown.Tier.StopObserving(TownUpgrade);
            _boundTown.DevelopmentScore.StopObserving(UpdateDevelopmentScore);
            _boundTown.DevelopmentTrend.StopObserving(UpdateDevelopmentTrend);
            _boundTown.GrowthTrend.StopObserving(UpdateGrowthTrend);
            _boundTown = null;
        }

        private void UnbindInventory()
        {
            ResetInventorySections();

            fundsText.text = "0";

            if (_boundInventory == null) return;

            _boundInventory.GoodUpdated -= OnGoodUpdated;
            _boundInventory.Funds.StopObserving(OnFundsUpdated);
            _boundInventory = null;
        }

        private void ResetInventorySections()
        {
            foreach (var section in inventorySections.Values)
            {
                section.Reset();
            }
        }

        private void SetUpInventorySections()
        {
            // go through each section and unlock the production buildings
            foreach (var good in _boundTown.Producer.AllProducers)
            {
                UnlockProducer(good);
            }
        }

        private void UnlockProducer(Good good)
        {
            var goodConfigData = _goodsConfig.Value.ConfigData;
            var goodTier = goodConfigData[good].Tier;
            var index = _boundTown.Producer.GetIndexOfProducedGood(good);
            var section = inventorySections[goodTier];
            section.UnlockProductionCell(index, good);
        }

        private void RefreshTownName(Tier tier)
        {
            townNameText.text = $"{_boundTown.Name} ({tier.ToRomanNumeral()})";
        }

        private void TownUpgrade(Tier tier)
        {
            RefreshTownName(tier);
            ShowSection(tier);
        }

        private void UpdateDevelopmentScore(float score)
        {
            developmentScore.SetDevelopment(score);
        }

        private void UpdateDevelopmentTrend(float trend)
        {
            var sign = trend > 0 ? "+" : "";
            developmentTrendText.text = $"{sign}{trend}%";
        }

        private void UpdateGrowthTrend(GrowthTrend obj)
        {
            developmentTrendIcon.sprite = _growthConfig.Value.ConfigData[obj].Icon;
        }

        private void HideSection(Tier tier)
        {
            inventorySections[tier].gameObject.SetActive(false);
        }

        private void ShowSection(Tier tier)
        {
            inventorySections[tier].gameObject.SetActive(true);
        }

        private void OnFundsUpdated(int funds)
        {
            fundsText.text = funds.ToString("N0");
        }

        private void OnGoodUpdated(Good good, int amount)
        {
            var tier = ConfigurationManager.Instance.GoodsConfig.ConfigData[good].Tier;
            var isProduced = _boundTown.Producer.IsProduced(good);

            if (isProduced)
            {
                var cellIndex = _boundTown.Producer.GetIndexOfProducedGood(good);
                inventorySections[tier].UpdateProducedGood(good, amount, cellIndex);
            }
            else
            {
                inventorySections[tier].UpdateForeignGood(good, amount);
            }
        }
    }
}