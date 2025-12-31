using AYellowpaper.SerializedCollections;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Elements;
using Features.Goods.Config;
using Features.Towns.Production.Logic;
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
        private SerializedDictionary<Tier, InventoryTierGroup> tierGroups;

        private GoodsResources _goodsConfig;
        private Town _town;

        public override void Initialize()
        {
            _goodsConfig = ResourceManager.Instance.GoodsResources;
            foreach (var row in tierGroups.Values)
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

            town.ProductionManager.ProductionAdded += OnProducerAdded;
        }


        public override void Unbind(Town town)
        {
            foreach (var row in tierGroups.Values)
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
            tierGroups[goodTier].UpdateGood(good, amount);
        }

        private void OnTownTierChanged(Tier townTier)
        {
            foreach (var (rowTier, row) in tierGroups)
            {
                row.SetLocked(rowTier > townTier);
            }
        }

        private void OnProducerAdded(Producer producer)
        {
            // remove good if we build a producer with a good in the inventory
            tierGroups[producer.Tier].UpdateGood(producer.ProducedGood, 0);
        }
        
        private void OnInventoryCellClicked(InventoryCellBase cell)
        {
            inventoryCellClicked.Invoke(cell, TradeType.Sell);
        }
    }
}