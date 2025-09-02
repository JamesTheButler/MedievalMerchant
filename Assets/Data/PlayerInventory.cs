using System.Collections.Generic;

namespace Data
{
    public class PlayerInventory : Inventory
    {
        private readonly Dictionary<Tier, int> _slotsPerTier = new()
        {
            { Tier.Tier1, 0 },
            { Tier.Tier2, 0 },
            { Tier.Tier3, 0 },
        };

        public void AddSlots(Tier tier, int count)
        {
            _slotsPerTier[tier] += count;
        }

        public bool CanAdd(Good good)
        {
            var tier = GoodsInfoManager.Value.ConfigData[good].Tier;
            var currentCount = GoodsPerTier()[tier];
            var availableCount = _slotsPerTier[tier] - currentCount;
            return availableCount > 0;
        }
    }
}