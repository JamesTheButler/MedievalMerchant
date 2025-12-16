using Common;
using Common.Types;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Logic;

namespace Features.Levels.Conditions.Model
{
    public sealed class TownTierWinCondition : IWinCondition
    {
        public Progress Progress { get; }

        public Tier TargetTier => _data.TargetTier;
        public int TargetCount => _data.TargetCount;
        public ConditionType Type => _data.Type;
        public string Description => _data.Description;

        private readonly TownTierWinConditionData _data;

        public TownTierWinCondition(TownTierWinConditionData data)
        {
            _data = data;

            Progress = new Progress(TargetCount, FormatProgress);
        }

        private string FormatProgress(int currentValue, int maxValue)
        {
            return $"{currentValue}/{maxValue} Tier {TargetTier.ToRomanNumeral()} towns";
        }
    }
}