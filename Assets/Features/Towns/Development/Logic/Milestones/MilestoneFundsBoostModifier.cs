using Common;
using Common.Modifiable;

namespace Features.Towns.Development.Logic.Milestones
{
    public sealed class MilestoneFundsBoostModifier : BasePercentageModifier
    {
        public MilestoneFundsBoostModifier(float value, MilestoneManager.UpgradeTime upgradeTime)
            : base(value, GetDescription(upgradeTime)) { }

        private static string GetDescription(MilestoneManager.UpgradeTime upgradeTime)
        {
            var percentage = upgradeTime.DevelopmentScore.ToPercentString();
            var tier = upgradeTime.Tier.ToRomanNumeral();
            return $"from Milestone ({percentage}, Tier {tier})";
        }
    }
}