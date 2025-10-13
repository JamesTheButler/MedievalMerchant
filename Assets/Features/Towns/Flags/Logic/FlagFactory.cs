using System.Collections.Generic;
using Common;
using Common.Types;

namespace Features.Towns.Flags.Logic
{
    public sealed class FlagFactory
    {
        private readonly List<(FlagColor Color, FlagShape Shape)> _createdFlags = new();

        public FlagInfo CreateFlagInfo(Good good)
        {
            (FlagColor Color, FlagShape Shape) candidate;
            do
            {
                var candidateColor = EnumExtensions.GetRandom<FlagColor>();
                var candidateShape = EnumExtensions.GetRandom<FlagShape>();
                candidate = (candidateColor, candidateShape);
            } while (_createdFlags.Contains(candidate));

            _createdFlags.Add(candidate);
            return new FlagInfo(candidate.Color, candidate.Shape, good);
        }
    }
}