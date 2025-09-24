using System;
using AYellowpaper.SerializedCollections;
using Common;
using Data;
using Data.Configuration;
using Data.Towns;
using Data.Trade;
using NaughtyAttributes;
using TMPro;
using UI.Popups;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.InventoryUI
{
    public sealed class TownInventoryUI : MonoBehaviour, IPointerClickHandler
    {
        [Header("Events")]
        [SerializeField]
        private UnityEvent<InventoryCellBase> inventoryCellClicked;

        [SerializeField]
        private UnityEvent<ProductionCell> tier1UpgradeButtonClicked;

        [SerializeField]
        private UnityEvent<ProductionCell> tier2UpgradeButtonClicked;

        [SerializeField]
        private UnityEvent<ProductionCell> tier3UpgradeButtonClicked;

        [SerializeField]
        private UnityEvent<Town> travelButtonClicked;

        [Header("Header UI Elements")]
        [SerializeField, Required]
        private TMP_Text townNameText;
        [SerializeField, Required]
        private DevelopmentGauge developmentGauge;

        [Header("Inventory UI Elements")]
        [SerializeField, Required]
        private TMP_Text fundsText;

        [SerializeField, SerializedDictionary("Tier", "Section")]
        private SerializedDictionary<Tier, TownInventorySection> inventorySections;

        private Town _town;
        private DevelopmentManager _developmentManager;
        private Inventory _inventory;

        private readonly Lazy<GoodsConfig> _goodsConfig =
            new(() => ConfigurationManager.Instance.GoodsConfig);

        public void Initialize()
        {
            foreach (var section in inventorySections.Values)
            {
                section.InventoryCellClicked += productionCell => inventoryCellClicked.Invoke(productionCell);
            }

            inventorySections[Tier.Tier1].UpgradeButtonClicked += tier1UpgradeButtonClicked.Invoke;
            inventorySections[Tier.Tier2].UpgradeButtonClicked += tier2UpgradeButtonClicked.Invoke;
            inventorySections[Tier.Tier3].UpgradeButtonClicked += tier3UpgradeButtonClicked.Invoke;
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
            _town.Upgrade();
        }

        public void TravelHere()
        {
            travelButtonClicked?.Invoke(_town);
        }

        private void BindTown(Town town)
        {
            _town = town;

            BindInventory(_town.Inventory);

            // don't invoke directly as we want to go through all tiers manually in the right order
            _town.Tier.Observe(TownUpgrade, false);
            for (var tier = Tier.Tier1; tier <= _town.Tier; tier++)
            {
                ShowSection(tier);
            }

            RefreshTownName(_town.Tier);

            developmentGauge.Bind(_developmentManager);
        }


        private void BindInventory(Inventory inventory)
        {
            HideSection(Tier.Tier2);
            HideSection(Tier.Tier3);

            SetUpInventorySections();

            _town.Producer.ProductionAdded += OnProducerAdded;

            inventory.GoodUpdated += OnGoodUpdated;
            inventory.Funds.Observe(OnFundsUpdated);

            foreach (var (good, amount) in inventory.Goods)
            {
                OnGoodUpdated(good, amount);
            }

            _inventory = inventory;
            _developmentManager = _town.DevelopmentManager;
        }

        public void Unbind()
        {
            UnbindTown();
            UnbindInventory();
        }

        private void UnbindTown()
        {
            if (_town == null) return;

            _town.Tier.StopObserving(TownUpgrade);
            developmentGauge.Unbind();
           
            _town.Producer.ProductionAdded -= OnProducerAdded;
            _town = null;
        }

        private void UnbindInventory()
        {
            ResetInventorySections();

            fundsText.text = "0";

            if (_inventory == null) return;

            _inventory.GoodUpdated -= OnGoodUpdated;
            _inventory.Funds.StopObserving(OnFundsUpdated);
            _inventory = null;
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
            foreach (var good in _town.Producer.AllProducers)
            {
                OnProducerAdded(good);
            }

            inventorySections[Tier.Tier1].EnableProductionCellUpgradeButtons(true);

            _town.Producer.ProductionAdded += OnProducerAdded;
        }

        private void OnProducerAdded(Good good)
        {
            var goodConfigData = _goodsConfig.Value.ConfigData;
            var goodTier = goodConfigData[good].Tier;
            var index = _town.Producer.GetIndexOfProducedGood(good);
            var section = inventorySections[goodTier];
            section.UnlockProductionCell(index, good);

            switch (goodTier)
            {
                case Tier.Tier1:
                {
                    var tier2Section = inventorySections[Tier.Tier2];
                    tier2Section.EnableProductionCellUpgradeButton(index, true);

                    break;
                }

                case Tier.Tier2:
                    break;

                case Tier.Tier3:
                    // TBD
                    break;
                default: break;
            }
        }

        private void RefreshTownName(Tier tier)
        {
            townNameText.text = $"{_town.Name} ({tier.ToRomanNumeral()})";
        }

        private void TownUpgrade(Tier tier)
        {
            RefreshTownName(tier);
            ShowSection(tier);
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
            if (_town == null)
            {
                Debug.LogError($"{nameof(OnGoodUpdated)} was called while no town was bound");
                return;
            }

            var tier = ConfigurationManager.Instance.GoodsConfig.ConfigData[good].Tier;
            var isProduced = _town.Producer.IsProduced(good);

            if (isProduced)
            {
                var cellIndex = _town.Producer.GetIndexOfProducedGood(good);
                inventorySections[tier].UpdateProducedGood(good, amount, cellIndex);
            }
            else
            {
                inventorySections[tier].UpdateForeignGood(good, amount);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PopupManager.Instance.HideActive();
        }
    }
}