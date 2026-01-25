using System;
using Common.Utility;
using UnityEngine;

namespace Features.Towns.Development.Config.Milestones
{
    [Serializable]
    public sealed class ProductionBoostUpgradeData : MilestoneUpgradeData
    {
        [field: SerializeField]
        public float ProductionBoost { get; private set; }

        public override string Description =>
            $"Boosts the towns good production by {ProductionBoost.ToPercentString()}.";
    }
}