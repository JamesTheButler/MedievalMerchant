using AYellowpaper.SerializedCollections;
using Common.Infrastructure;
using Common.Types;
using Features.Goods.Config;
using Features.Towns.Production.Logic;
using UnityEngine;

namespace Features.Towns.UI
{
    public sealed class TownUIInventorySection : TownUISection
    {
        [SerializeField, SerializedDictionary("Tier", "Section")]
        private SerializedDictionary<Tier, InventoryTierGroup> tierGroups;

        private GoodResources _goodConfig;
        private Town _town;

        public override void Initialize()
        {
            _goodConfig = ResourceManager.Instance.GoodResources;
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

            town.ProductionManager.ProductionAdded.Observe(OnProducerAdded);
        }

        public override void Unbind(Town town)
        {
            foreach (var row in tierGroups.Values)
            {
                row.Reset();
            }

            town.Tier.StopObserving(OnTownTierChanged);
            town.ProductionManager.ProductionAdded.StopObserving(OnProducerAdded);
            town.Inventory.GoodUpdated -= UpdateGood;
        }

        private void UpdateGood(Good good, int amount)
        {
            // ignore goods produced in this town. ProductionPanel handles that
            if (_town.ProductionManager.IsProduced(good))
                return;

            var goodTier = _goodConfig.ResourceData[good].Tier;
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
    }
}