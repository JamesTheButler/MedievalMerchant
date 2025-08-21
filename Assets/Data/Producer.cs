using System;
using System.Collections.Generic;

namespace Data
{
    public class Producer
    {
        // goods this producer is allowed to produce
        private readonly ProductionTable _possibleProductionTable;
        // goods this producer is actually producing
        private readonly ProductionTable _currentProductionTable;

        private Tier? _tier;
        
        public Producer(ProductionTable possibleProductionTable)
        {
            _possibleProductionTable = possibleProductionTable;
            _currentProductionTable = possibleProductionTable;
            UpgradeTier(Tier.Tier1);
        }

        public IEnumerable<Good> Produce()
        {
            return new List<Good>();
        }

        // Tier 1: 1 Tier1 Good
        // Tier 2: 2 Tier1 Good + 1 Tier2 Good
        // Tier 3: 2 Tier1 Good + 2 Tier2 Good + 1 Tier3 Good
        private void UpgradeTier(Tier tier)
        {
            if (_tier == tier) return;

            switch (tier)
            {
                case Tier.Tier1: break;
                case Tier.Tier2: break;
                case Tier.Tier3: break;
                default: throw new ArgumentOutOfRangeException(nameof(tier), tier, null);
            }
            
            _tier = tier;
        }
    }
}