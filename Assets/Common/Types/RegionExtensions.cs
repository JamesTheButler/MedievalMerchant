using System;
using System.Collections.Generic;

namespace Common.Types
{
    public static class RegionExtensions
    {
        public static Region GetRandom(this Regions regions)
        {
            var activeRegions = new List<Region>();

            foreach (Region region in Enum.GetValues(typeof(Region)))
            {
                var flag = (Regions)(1 << (int)region);
                if (regions.HasFlag(flag))
                {
                    activeRegions.Add(region);
                }
            }

            return activeRegions.Count == 0
                ? Region.Forest
                : activeRegions[new Random().Next(activeRegions.Count)];
        }

        public static Regions AsRegions(this Region region)
        {
            return (Regions)(1 << (int)region);
        }
    }
}