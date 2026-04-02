using System.Collections.Generic;
using Common.Types;
using Common.Utility;

namespace Features.Goods
{
    public static class GoodPoolExtensions
    {
        public static IReadOnlyCollection<Good> Get(this GoodPool pool, Tier tier)
        {
            return tier switch
            {
                Tier.Tier1 => pool.Tier1Goods,
                Tier.Tier2 => pool.Tier2Goods,
                Tier.Tier3 => pool.Tier3Goods,
                _ => pool.Tier1Goods
            };
        }

        public static Good GetRandom(this GoodPool pool, Tier tier)
        {
            return pool.Get(tier).GetRandom();
        }

        public static int GetSize(this GoodPool pool, Tier tier)
        {
            return pool.Get(tier).Count;
        }
    }
}