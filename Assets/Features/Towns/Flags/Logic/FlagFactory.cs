using System.Collections.Generic;
using Common.Types;
using Common.Utility;

namespace Features.Towns.Flags.Logic
{
    public sealed class FlagFactory
    {
        private readonly List<FlagInfo> _createdFlags = new();

        private readonly Dictionary<Region, FlagColor> _colorMap = new()
        {
            { Region.Fields, FlagColor.Yellow },
            { Region.Forest, FlagColor.Green },
            { Region.Ocean, FlagColor.Blue },
            { Region.Mountains, FlagColor.Red }
        };

        public FlagInfo CreateFlagInfo(Region townRegion)
        {
            FlagInfo candidate;
            do
            {
                var candidateColor = _colorMap[townRegion];
                var candidateShape = EnumExtensions.GetRandom<FlagShape>();
                candidate = new FlagInfo(candidateColor, candidateShape, townRegion);
            } while (_createdFlags.Contains(candidate));

            _createdFlags.Add(candidate);
            return candidate;
        }
    }
}