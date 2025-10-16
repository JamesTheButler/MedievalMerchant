using System;
using System.Collections.Generic;

namespace Common.Types
{
    public static class RegionExtensions
    {
        public static Regions GetRandom(this Regions regions)
        {
            var activeRegions = new List<Regions>();
            foreach (Regions region in Enum.GetValues(typeof(Regions)))
            {
                if (regions.HasFlag(region))
                    activeRegions.Add(region);
            }

            if (activeRegions.Count == 0)
            {
                throw new ArgumentException("No valid regions specified.", nameof(regions));
            }

            return activeRegions[new Random().Next(activeRegions.Count)];
        }
    }
}