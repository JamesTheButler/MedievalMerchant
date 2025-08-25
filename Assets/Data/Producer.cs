using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    public class Producer
    {
        public IEnumerable<Good> ProducedGoods => _producedGoods;

        private const int BaseProduction = 4;
        
        private readonly HashSet<Good> _producedGoods = new();
        private readonly ProductionTable _productionTable;

        private Tier _tier;
        private float _multiplier = 1f;

        public Producer(ProductionTable productionTable)
        {
            _productionTable = productionTable;
            UpgradeTier(Tier.Tier1);
        }

        public IDictionary<Good, int> Produce()
        {
            return _producedGoods.ToDictionary(good => good, _ => (int)(BaseProduction * _multiplier * (int)_tier));
        }

        // Tier 1: 1 Tier1 Good
        // Tier 2: 2 Tier1 Good + 1 Tier2 Good
        // Tier 3: 2 Tier1 Good + 2 Tier2 Good + 1 Tier3 Good
        public void UpgradeTier(Tier tier)
        {
            if (_tier >= tier) return;

            switch (tier)
            {
                case Tier.Tier1:
                    _producedGoods.Add(_productionTable.Tier1Goods.PickRandom());
                    break;
                case Tier.Tier2:
                    _producedGoods.Add(_productionTable.Tier1Goods.Except(_producedGoods).ToList().PickRandom());
                    _producedGoods.Add(_productionTable.Tier2Goods.Except(_producedGoods).ToList().PickRandom());
                    break;
                case Tier.Tier3:
                    _producedGoods.Add(_productionTable.Tier2Goods.Except(_producedGoods).ToList().PickRandom());
                    _producedGoods.Add(_productionTable.Tier3Goods.PickRandom());
                    break;
                default:
                    Debug.LogError($"Tier {tier} is not supported");
                    break;
            }

            _tier = tier;
        }

        public void SetProductionMultiplier(float multiplier)
        {
            _multiplier = multiplier;
        }
    }
}