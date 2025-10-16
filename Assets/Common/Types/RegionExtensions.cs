using System;
using System.Linq;

namespace Common.Types
{
    public static class RegionExtensions
    {
        public static Region GetRandom(this Regions regions)
        {
            var activeRegions = Enum.GetValues(typeof(Regions))
                .Cast<Regions>()
                .Where(region => regions.HasFlag(region))
                .ToList();

            if (activeRegions.Count == 0)
            {
                return Region.Forest;
            }

            return (Region)(int)activeRegions[new Random().Next(activeRegions.Count)];
        }


        public static Regions AsRegions(this Region region)
        {
            return (Regions)(1 << (int)region);
        }
    }
}