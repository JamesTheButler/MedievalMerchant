using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.Types;
using Common.Utility;
using Features.Goods.Config;
using Features.Goods.Selector;

namespace Features.Towns.Production.Logic
{
    public sealed class ProductionManager
    {
        public ObservableEvent<Producer> ProductionAdded { get; } = new();
        public ObservableEvent<Producer, int> ProductionAddedIndexed { get; } = new();

        private readonly Town _town;
        private readonly GoodResources _goodResources = ResourceManager.Instance.GoodResources;
        private readonly Dictionary<Tier, Producer[]> _producers;
        private readonly List<IModifier> _productionModifiers = new();

        public ObservableSum ProductionBuildingCostModifiers { get; } = new();

        public ProductionManager(Town town)
        {
            _town = town;

            _producers = new Dictionary<Tier, Producer[]>
            {
                { Tier.Tier1, new Producer[] { null, null, null } },
                { Tier.Tier2, new Producer[] { null, null, null } },
                { Tier.Tier3, new Producer[] { null, null, null } }
            };
        }

        public IEnumerable<Producer> AllProducers => _producers[Tier.Tier1]
            .Concat(_producers[Tier.Tier2])
            .Concat(_producers[Tier.Tier3])
            .WhereNotNull();

        public bool IsProduced(Good good)
        {
            return _producers.Values.Any(producers => producers.Any(producer => producer?.ProducedGood == good));
        }

        public Producer[] GetProducers(Tier tier)
        {
            return _producers[tier];
        }

        public bool HasProducer(Tier tier, int index)
        {
            if (index < 0 || index >= _producers[tier].Length)
                return false;

            return _producers[tier][index] != null;
        }

        public int GetIndexOfProducedGood(Good good)
        {
            var tier = _goodResources.ResourceData[good].Tier;
            return GetProducers(tier)
                .ToList()
                .IndexOf(producer => producer?.ProducedGood == good);
        }

        public void AddProducer(Good good, int index)
        {
            if (!CanAddProducer(good, index))
                return;

            var tier = _goodResources.ResourceData[good].Tier;
            var producers = GetProducers(tier);
            var producer = new Producer(good, _town);
            producers[index] = producer;
            producer.ProductionRate.AddModifiers(_productionModifiers);
            ProductionAdded?.Invoke(producer);
            ProductionAddedIndexed?.Invoke(producer, index);
        }

        public void AddConstructionModifier(IModifier modifier)
        {
            ProductionBuildingCostModifiers.AddValue(modifier.Value);
        }

        public void RemoveConstructionModifier(IModifier modifier)
        {
            ProductionBuildingCostModifiers.RemoveValue(modifier.Value);
        }

        public void AddModifier(IModifier prodBoostModifier, IGoodSelector goodSelector)
        {
            _productionModifiers.Add(prodBoostModifier);
            foreach (var producer in AllProducers)
            {
                if (!goodSelector.Matches(producer.ProducedGood))
                    continue;

                producer.ProductionRate.AddModifier(prodBoostModifier);
            }
        }

        public void RemoveModifier(IModifier prodBoostModifier, IGoodSelector goodSelector)
        {
            _productionModifiers.Remove(prodBoostModifier);
            foreach (var producer in AllProducers)
            {
                if (!goodSelector.Matches(producer.ProducedGood))
                    continue;

                producer.ProductionRate.RemoveModifier(prodBoostModifier);
            }
        }

        private bool CanAddProducer(Good good, int index)
        {
            var tier = _goodResources.ResourceData[good].Tier;
            return !HasProducer(tier, index);
        }
    }
}