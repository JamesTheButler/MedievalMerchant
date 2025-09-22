using System;
using System.Linq;
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

        public override void Initialize()
        {
            _model = Model.Instance;
            foreach (var town in _model.Towns.Values)
            {
                town.Tier.Observe(_ => Evaluate());
            }
        }

        private void Evaluate()
        {
            var currentCount = _model.Towns.Values.Count(town => town.Tier.Value >= targetTier);

            IsCompleted.Value = currentCount >= targetCount;
        }
    }
}