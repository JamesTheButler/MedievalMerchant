using System;

namespace Features.Towns.Development.Config.Milestones
{
    /// <summary>
    /// When a town no longer regresses in its growth without player intervention.
    /// </summary>
    [Serializable]
    public sealed class SelfSufficienyUpgradeData : MilestoneUpgradeData
    {
        public override string Description => "The town will no longer decline over time.";
    }
}