using System;
using System.Linq;
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

namespace UI
{
    public sealed class TownInventoryUI : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<InventoryCell> inventoryCellClicked;

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
        private SerializedDictionary<Tier, InventorySection> inventorySections;

        private Town _boundTown;
        private Inventory _boundInventory;

        private readonly Lazy<Model> _model = new(() => Model.Instance);

        private readonly Lazy<GrowthTrendConfig> _growthConfig =
            new(() => ConfigurationManager.Instance.GrowthTrendConfig);

        public void Initialize()
        {
            foreach (var section in inventorySections.Values)
            {
                section.CellClicked += InvokeCellClicked;
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
            // TODO: we should hide/disable the Travel button if the player is in town
            _model.Value.Player.Location.CurrentTown = _boundTown;
        }

        private void BindTown(Town town)
        {
            _boundTown = town;

            BindInventory(_boundTown.Inventory);

            // don't invoke directly as we want to go through all tiers manually in the right order
            _boundTown.Tier.Observe(TownUpgrade, false);
            // upgrade each tier until current tier is reached
            for (var i = Tier.Tier1; i <= _boundTown.Tier; i++)
            {
                TownUpgrade(i);
            }

            _boundTown.DevelopmentScore.Observe(UpdateDevelopmentScore);
            _boundTown.DevelopmentTrend.Observe(UpdateDevelopmentTrend);
            _boundTown.GrowthTrend.Observe(UpdateGrowthTrend);
        }

        private void BindInventory(Inventory inventory)
        {
            ResetInventorySections();

            HideSection(Tier.Tier2);
            HideSection(Tier.Tier3);

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
            fundsText.text = "0";

            ResetInventorySections();

            if (_boundInventory == null) return;

            _boundInventory.GoodUpdated -= OnGoodUpdated;
            _boundInventory.Funds.StopObserving(OnFundsUpdated);
            _boundInventory = null;
        }

        private void ResetInventorySections()
        {
            foreach (var section in inventorySections.Values)
            {
                section.Initialize();
            }
        }

        private void TownUpgrade(Tier tier)
        {
            townNameText.text = $"{_boundTown.Name} ({tier.ToRomanNumeral()})";
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
            fundsText.text = funds.ToString();
        }

        private void OnGoodUpdated(Good good, int amount)
        {
            var tier = ConfigurationManager.Instance.GoodsConfig.ConfigData[good].Tier;

            var isProduced = _boundTown?.Production.Contains(good) ?? false;
            inventorySections[tier].UpdateGood(good, amount, isProduced);
        }

        private void InvokeCellClicked(InventoryCell cell)
        {
            inventoryCellClicked?.Invoke(cell);
        }
    }
}