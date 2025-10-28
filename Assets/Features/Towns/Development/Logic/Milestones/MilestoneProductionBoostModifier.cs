using Common;

namespace Features.Towns.Development.Logic.Milestones
{
    public sealed class MilestoneProductionBoostModifier : ProductionBoostModifier
    {
        public MilestoneProductionBoostModifier(float value, TownUpgradeManager.UpgradeTime upgradeTime)
            : base(value, GetDescription(upgradeTime)) { }

        private static string GetDescription(TownUpgradeManager.UpgradeTime upgradeTime)
        {
            var percentage = upgradeTime.DevelopmentScore.ToPercentString();
            var tier = upgradeTime.Tier.ToRomanNumeral();
            return $"Milestone ({percentage}, Tier {tier}";
        }
    }
}