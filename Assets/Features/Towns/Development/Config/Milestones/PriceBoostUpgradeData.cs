using System;
using UnityEngine;

namespace Features.Towns.Development.Config.Milestones
{
    [Serializable]
    public sealed class PriceBoostUpgradeData : MilestoneUpgradeData
    {
        [field: SerializeField]
        public float PriceBoostPercent { get; private set; }

        public override string Description => Loc.PriceBoost(PriceBoostPercent);
    }
}