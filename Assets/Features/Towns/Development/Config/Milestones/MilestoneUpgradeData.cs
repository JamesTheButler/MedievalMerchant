using System;

namespace Features.Towns.Development.Config.Milestones
{
    [Serializable]
    public abstract class MilestoneUpgradeData
    {
        public abstract string Description { get; }
    }
}