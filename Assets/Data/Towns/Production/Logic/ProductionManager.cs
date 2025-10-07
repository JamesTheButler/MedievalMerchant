using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data.Configuration;

namespace Data.Towns.Production.Logic
{
    public sealed class ProductionManager
    {
        public event Action<Producer> ProductionAdded;

        private readonly Town _town;

        private readonly Dictionary<Tier, Producer[]> _producers;

        public ProductionManager(Town town)
        {
            _town = town;

            _producers = new()
            {
                { Tier.Tier1, new Producer[] { null, null, null } },
                { Tier.Tier2, new Producer[] { null, null, null } },
                { Tier.Tier3, new Producer[] { null, null, null } },
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

        private readonly GoodsConfig _goodsConfig = ConfigurationManager.Instance.GoodsConfig;

        public Producer[] GetProducers(Tier tier)
        {
            return _producers[tier];
        }

        public int GetIndexOfProducedGood(Good good)
        {
            var tier = _goodsConfig.ConfigData[good].Tier;
            return GetProducers(tier)
                .ToList()
                .IndexOf(producer => producer?.ProducedGood == good);
        }

        public bool CanAddProducer(Good good, int index)
        {
            var tier = _goodsConfig.ConfigData[good].Tier;
            return GetProducers(tier)[index] == null;
        }

        public void AddProducer(Good good, int index)
        {
            if (!CanAddProducer(good, index)) return;

            var tier = _goodsConfig.ConfigData[good].Tier;
            var producers = GetProducers(tier);
            var producer = new Producer(good, _town);
            producers[index] = producer;
            ProductionAdded?.Invoke(producer);
        }

        public void Produce()
        {
            foreach (var producer in AllProducers)
            {
                producer.Produce();
            }
        }
    }
}