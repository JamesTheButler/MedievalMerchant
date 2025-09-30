using System;
using System.Linq;
using Common;
using Data;
using Data.Configuration;
using UnityEngine;

namespace Levels.Conditions
{
    [Serializable]
    [CreateAssetMenu(
        fileName = nameof(TownTierWinCondition),
        menuName = AssetMenu.ConditionsFolder + nameof(TownTierWinCondition))]
    public sealed class TownTierWinCondition : WinCondition
    {
        [SerializeField]
        private Tier targetTier;

        [SerializeField]
        private int targetCount;

        private Model _model;

        public override ConditionType Type => ConditionType.TownTierWinCondition;

        public override string Description => $"Develop {targetCount} towns to Tier {targetTier.ToRomanNumeral()}.";

        public override void Initialize()
        {
            _model = Model.Instance;
            Progress = new Progress(targetCount, FormatProgress);

            foreach (var town in _model.Towns.Values)
            {
                town.Tier.Observe(_ => Evaluate());
            }
        }

        private void Evaluate()
        {
            var currentCount = _model.Towns.Values.Count(town => town.Tier.Value >= targetTier);
            Progress.SetProgress(currentCount);
        }

        private string FormatProgress(int currentValue, int maxValue)
        {
            return $"{currentValue}/{maxValue} Tier {targetTier.ToRomanNumeral()} towns";
        }
    }
}