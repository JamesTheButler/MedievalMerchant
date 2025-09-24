#nullable enable
using System;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Configuration
{
    [Serializable]
    public sealed class TownGoodConfigTable<T>
    {
        [SerializeField]
        private T town1Good1 = default!;

        [SerializeField]
        private T town2Good1 = default!;

        [SerializeField]
        private T town2Good2 = default!;

        [SerializeField]
        private T town3Good1 = default!;

        [SerializeField]
        private T town3Good2 = default!;

        [SerializeField]
        private T town3Good3 = default!;

        public T Get(Tier townTier, Tier goodTier, T fallbackValue = default!)
        {
            if (goodTier > townTier) return fallbackValue;

            return (townTier, goodTier) switch
            {
                (Tier.Tier1, Tier.Tier1) => town1Good1,
                (Tier.Tier2, Tier.Tier1) => town2Good1,
                (Tier.Tier2, Tier.Tier2) => town2Good2,
                (Tier.Tier3, Tier.Tier1) => town3Good1,
                (Tier.Tier3, Tier.Tier2) => town3Good2,
                (Tier.Tier3, Tier.Tier3) => town3Good3,
                _ => fallbackValue
            };
        }
    }
}