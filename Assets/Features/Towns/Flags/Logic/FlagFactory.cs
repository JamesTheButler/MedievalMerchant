using System.Collections.Generic;
using Common.Types;
using Common.Utility;

namespace Features.Towns.Flags.Logic
{
    public sealed class FlagFactory
    {
        private readonly List<FlagInfo> _createdFlags = new();

        public FlagInfo CreateFlagInfo(Region townRegion)
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