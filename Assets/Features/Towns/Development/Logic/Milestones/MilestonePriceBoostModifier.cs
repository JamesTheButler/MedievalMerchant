using Common;
using Common.Modifiable;

namespace Features.Towns.Development.Logic.Milestones
{
    public sealed class MilestonePriceBoostModifier : BasePercentageModifier
    {
        public MilestonePriceBoostModifier(float value, TownUpgradeManager.UpgradeTime upgradeTime) : base(value,
            GetDescription(upgradeTime)) { }

        private static string GetDescription(TownUpgradeManager.UpgradeTime upgradeTime)
        {
            var percentage = upgradeTime.DevelopmentScore.ToPercentString();
            var tier = upgradeTime.Tier.ToRomanNumeral();
            return $"Milestone ({percentage}, Tier {tier}";
        }
    }
}