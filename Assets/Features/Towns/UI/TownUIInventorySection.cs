using AYellowpaper.SerializedCollections;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Elements;
using Features.Goods.Config;
using Features.Towns.UI.Inventory;
using Features.Trade;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Towns.UI
{
    public sealed class TownUIInventorySection : TownUISection
    {
        [SerializeField]
        private UnityEvent<InventoryCellBase, TradeType> inventoryCellClicked;

        [SerializeField, SerializedDictionary("Tier", "Section")]
        private SerializedDictionary<Tier, InventoryTierRow> rows;

        private GoodsResources _goodsConfig;
        private Town _town;

        public override void Initialize()
        {
            _goodsConfig = ResourceManager.Instance.GoodsResources;
            foreach (var row in rows.Values)
            {
                row.InventoryCellClicked += OnInventoryCellClicked;
            }
        }

        public override void CleanUp() { }

        public override void Bind(Town town)
        {
            _town = town;
            town.Tier.Observe(OnTownTierChanged);

            town.Inventory.GoodUpdated += UpdateGood;
            foreach (var (good, amount) in town.Inventory.Goods)
            {
                UpdateGood(good, amount);
            }
        }

        public override void Unbind(Town town)
        {
            foreach (var row in rows.Values)
            {
                row.Reset();
            }

            town.Tier.StopObserving(OnTownTierChanged);
            town.Inventory.GoodUpdated -= UpdateGood;
        }

        private void UpdateGood(Good good, int amount)
        {
            // ignore goods produced in this town. ProductionPanel handles that
            if (_town.ProductionManager.IsProduced(good))
                return;

            var goodTier = _goodsConfig.ResourceData[good].Tier;
            rows[goodTier].UpdateGood(good, amount);
        }

        private void OnTownTierChanged(Tier townTier)
        {
            foreach (var (rowTier, row) in rows)
            {
                row.SetLocked(rowTier > townTier);
            }
        }

        private void OnInventoryCellClicked(InventoryCellBase cell)
        {
            inventoryCellClicked.Invoke(cell, TradeType.Sell);
        }
    }
}