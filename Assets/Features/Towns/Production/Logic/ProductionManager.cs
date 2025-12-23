using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;
using Common.Utility;
using Features.Goods;
using Features.Goods.Config;

namespace Features.Towns.Production.Logic
{
    public sealed class ProductionManager
    {
        public event Action<Producer> ProductionAdded;

        private readonly Town _town;
        private readonly GoodsResources _goodsResources = ResourceManager.Instance.GoodsResources;
        private readonly Dictionary<Tier, Producer[]> _producers;
        private readonly List<IModifier> _productionModifiers = new();

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

        public int GetIndexOfProducedGood(Good good)
        {
            var tier = _goodsResources.ConfigData[good].Tier;
            return GetProducers(tier)
                .ToList()
                .IndexOf(producer => producer?.ProducedGood == good);
        }

        public bool CanAddProducer(Good good, int index)
        {
            var tier = _goodsResources.ConfigData[good].Tier;
            return GetProducers(tier)[index] == null;
        }

        public void AddProducer(Good good, int index)
        {
            if (!CanAddProducer(good, index)) return;

            var tier = _goodsResources.ConfigData[good].Tier;
            var producers = GetProducers(tier);
            var producer = new Producer(good, _town);
            producers[index] = producer;
            producer.ProductionRate.AddModifiers(_productionModifiers);
            ProductionAdded?.Invoke(producer);
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
    }
}