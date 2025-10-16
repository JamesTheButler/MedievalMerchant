using System.Collections.Generic;
using Common;
using Common.Types;
using Features.Towns.Flags.Config;

namespace Features.Towns.Flags.Logic
{
    public sealed class FlagFactory
    {
        private readonly List<FlagInfo> _createdFlags = new();

        public FlagInfo CreateFlagInfo(Regions townRegion)
        {
            FlagInfo candidate;
            do
            {
                var candidateColor = EnumExtensions.GetRandom<FlagColor>();
                var candidateShape = EnumExtensions.GetRandom<FlagShape>();
                candidate = new FlagInfo(candidateColor, candidateShape, townRegion);
            } while (_createdFlags.Contains(candidate));

            _createdFlags.Add(candidate);
            return candidate;
        }
    }
}