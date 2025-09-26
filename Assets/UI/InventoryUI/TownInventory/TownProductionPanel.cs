using System;
using AYellowpaper.SerializedCollections;
using Data;
using Data.Configuration;
using Data.Towns;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace UI.InventoryUI.TownInventory
{
    public sealed class TownProductionPanel : MonoBehaviour
    {
        [SerializeField, Required]
        private ArrowDrawer tier2Arrows;

        [SerializeField, Required]
        private ArrowDrawer tier3Arrows;

        [SerializeField, SerializedDictionary("Tier", "Row")]
        private SerializedDictionary<Tier, ProductionTierRow> rows;

        [Header("Events")]
        [SerializeField]
        private UnityEvent<ProductionCell> productionCellClicked;

        [SerializeField]
        private UnityEvent<ProductionCell> tier1UpgradeButtonClicked;

        [SerializeField]
        private UnityEvent<ProductionCell> tier2UpgradeButtonClicked;

        [SerializeField]
        private UnityEvent<ProductionCell> tier3UpgradeButtonClicked;

        private Town _town;
        private readonly Lazy<GoodsConfig> _goodsConfig = new(() => ConfigurationManager.Instance.GoodsConfig);


        public void Initialize()
        {
            rows[Tier.Tier1].UpgradeButtonClicked += tier1UpgradeButtonClicked.Invoke;
            rows[Tier.Tier2].UpgradeButtonClicked += tier2UpgradeButtonClicked.Invoke;
            rows[Tier.Tier3].UpgradeButtonClicked += tier3UpgradeButtonClicked.Invoke;

            foreach (var section in rows.Values)
            {
                section.ProductionCellClicked += productionCell => productionCellClicked.Invoke(productionCell);
            }
        }

        public void Bind(Town town)
        {
            _town = town;

            BindTownTier();
            BindProducer();
            BindInventory();
        }

        public void Unbind()
        {
            foreach (var row in rows.Values)
            {
                row.Reset();
            }

            tier2Arrows.ClearArrows();
            tier3Arrows.ClearArrows();
            
            HideAllRows();

            if (_town == null) return;

            _town.Tier.StopObserving(ShowRow);
            _town.Producer.ProductionAdded -= OnProducerAdded;
            _town.Inventory.GoodUpdated -= UpdateGood;
            _town = null;
        }

        private void BindTownTier()
        {
            // don't invoke directly as we want to go through all tiers manually in the right order
            _town.Tier.Observe(ShowRow, false);
            for (var tier = Tier.Tier1; tier <= _town.Tier; tier++)
            {
                ShowRow(tier);
            }

            // TODO: should only enable up to a max amount == town.availableGoods
            rows[Tier.Tier1].EnableProductionCellUpgradeButtons(true);
        }

        private void BindInventory()
        {
            _town.Inventory.GoodUpdated += UpdateGood;
        }

        private void UpdateGood(Good good, int amount)
        {
            // goods NOT produced here, are handled by TownInventoryPanel
            if (!_town.Producer.IsProduced(good))
                return;

            var goodTier = _goodsConfig.Value.ConfigData[good].Tier;
            var cellIndex = _town.Producer.GetIndexOfProducedGood(good);
            rows[goodTier].UpdateProducedGood(good, amount, cellIndex);
        }

        private void BindProducer()
        {
            foreach (var good in _town.Producer.AllProducers)
            {
                OnProducerAdded(good);
            }

            _town.Producer.ProductionAdded += OnProducerAdded;
        }

        private void OnProducerAdded(Good good)
        {
            var goodConfigData = _goodsConfig.Value.ConfigData;
            var goodTier = goodConfigData[good].Tier;
            var index = _town.Producer.GetIndexOfProducedGood(good);
            var section = rows[goodTier];
            section.UnlockProductionCell(index, good);

            switch (goodTier)
            {
                case Tier.Tier1:
                {
                    var tier2Section = rows[Tier.Tier2];
                    tier2Section.EnableProductionCellUpgradeButton(index, true);
                    tier2Arrows.AddArrow(index, index);
                    break;
                }

                case Tier.Tier2:
                    // TODO: if producers contains enough for a Tier3 recipe, set up arrows and enable button
                    tier3Arrows.AddArrow(index, 0);
                    tier3Arrows.AddArrow(index, 1);
                    tier3Arrows.AddArrow(index, 2);
                    break;

                case Tier.Tier3:
                    break;

                default: break;
            }
        }

        private void HideAllRows()
        {
            foreach (var row in rows.Values)
            {
                row.gameObject.SetActive(false);
            }
        }

        private void ShowRow(Tier tier)
        {
            rows[tier].gameObject.SetActive(true);
        }
    }
}