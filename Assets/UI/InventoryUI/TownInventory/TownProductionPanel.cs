using System;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Common;
using Data;
using Data.Configuration;
using Data.Goods.Recipes.Config;
using Data.Towns;
using Data.Towns.Production.Logic;
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
        private readonly Lazy<RecipeConfig> _recipeConfig = new(() => ConfigurationManager.Instance.RecipeConfig);


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

            _town.Tier.StopObserving(OnTierChanged);
            _town.ProductionManager.ProductionAdded -= OnProducerAdded;
            _town.Inventory.GoodUpdated -= UpdateGood;
            _town = null;
        }

        private void BindTownTier()
        {
            // don't invoke directly as we want to go through all tiers manually in the right order
            _town.Tier.Observe(OnTierChanged, false);
            for (var tier = Tier.Tier1; tier <= _town.Tier; tier++)
            {
                OnTierChanged(tier);
            }

            // TODO - POLISH: should only enable up to a max amount == town.availableGoods
            rows[Tier.Tier1].EnableProductionCellUpgradeButtons(true);
        }

        private void BindInventory()
        {
            _town.Inventory.GoodUpdated += UpdateGood;
        }

        private void UpdateGood(Good good, int amount)
        {
            // goods NOT produced here, are handled by TownInventoryPanel
            if (!_town.ProductionManager.IsProduced(good))
                return;

            var goodTier = _goodsConfig.Value.ConfigData[good].Tier;
            var cellIndex = _town.ProductionManager.GetIndexOfProducedGood(good);
            rows[goodTier].UpdateProducedGood(good, amount, cellIndex);
        }

        private void BindProducer()
        {
            foreach (var producer in _town.ProductionManager.AllProducers)
            {
                OnProducerAdded(producer);
            }

            _town.ProductionManager.ProductionAdded += OnProducerAdded;
        }

        private void OnProducerAdded(Producer producer)
        {
            var good = producer.ProducedGood;
            var goodConfigData = _goodsConfig.Value.ConfigData;
            var goodTier = goodConfigData[good].Tier;
            var index = _town.ProductionManager.GetIndexOfProducedGood(good);
            var section = rows[goodTier];
            section.UnlockProductionCell(index, good);

            switch (goodTier)
            {
                case Tier.Tier1:
                {
                    var tier2Section = rows[Tier.Tier2];
                    tier2Section.EnableProductionCellUpgradeButton(index, true);
                    RefreshTier2Arrows();
                    break;
                }

                case Tier.Tier2:
                    var tier3Section = rows[Tier.Tier3];
                    tier3Section.EnableProductionCellUpgradeButton(index, true);
                    RefreshTier3Arrows();
                    break;

                case Tier.Tier3:
                    break;
            }
        }

        public void RefreshTier2Arrows()
        {
            tier2Arrows.ClearArrows();
            if (_town.Tier < Tier.Tier2) return;

            var t1Producers = _town.ProductionManager.GetProducers(Tier.Tier1);
            for (var i = 0; i < t1Producers.Length; i++)
            {
                if (t1Producers[i] == null) continue;

                tier2Arrows.AddArrow(i, i);
            }
        }

        // TODO - POLISH: GetAllTuples is really expensive
        public void RefreshTier3Arrows()
        {
            tier3Arrows.ClearArrows();
            if (_town.Tier < Tier.Tier3) return;

            var t2Producers = _town.ProductionManager.GetProducers(Tier.Tier2);
            for (var i = 0; i < t2Producers.Length; i++)
            {
                if (t2Producers[i] == null) continue;

                tier3Arrows.AddArrow(i, i);
            }
        }

        private void HideAllRows()
        {
            foreach (var row in rows.Values)
            {
                row.gameObject.SetActive(false);
            }
        }

        private void OnTierChanged(Tier tier)
        {
            // show row for tier
            rows[tier].gameObject.SetActive(true);
            RefreshTier2Arrows();
            RefreshTier3Arrows();
        }
    }
}