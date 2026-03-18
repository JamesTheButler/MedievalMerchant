using System;
using Common.Infrastructure;
using Features.Localization.Data;

namespace Features.Towns.Development.Config.Milestones
{
    [Serializable]
    public abstract class MilestoneUpgradeData
    {
        public abstract string Description { get; }

        protected TownMilestonesLocalizationResources Loc => ResourceManager.Instance
            .LocalizationResources
            .Town
            .Milestones;
    }
}