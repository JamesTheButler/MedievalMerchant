using System;
using Common.Utility;
using UnityEngine;

namespace Features.Towns.Development.Config.Milestones
{
    [Serializable]
    public sealed class FundsBoostUpgradeData : MilestoneUpgradeData
    {
        [field: SerializeField]
        public float FundsBoost { get; private set; }

        public override string Description => $"Increases the towns coin production by {FundsBoost.ToPercentString()}.";
    }
}