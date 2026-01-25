using System;
using Common.Utility;
using UnityEngine;

namespace Features.Towns.Development.Config.Milestones
{
    /// <summary>
    /// Upgrade for automatically transferring part of the towns fund-production to the player.
    /// </summary>
    [Serializable]
    public sealed class DividendsUpgradeData : MilestoneUpgradeData
    {
        [field: SerializeField]
        public float DividendsPercentage { get; private set; }

        public override string Description =>
            $"Receive {DividendsPercentage.ToPercentString()} of the towns coin production.";
    }
}